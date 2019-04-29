namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using RestSharp;
    using SprintCrowd.Backend.Application;
    using SprintCrowd.Backend.Models;
    using SprintCrowdBackEnd.Application;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Infrastructure.Persistence;
    using SprintCrowdBackEnd.Web.Account;
    using SprintCrowdBackEnd.Web.PushNotification;

    /// ONLY REPOSITORIES WILL ACCESS THE DATABASE
    /// NO DIRECT ACCESS FROM SERVICES OR CONTROLLERS ALLOWED.
    /// <summary>
    ///  user repository for getting and modifing users.
    /// </summary>
    public class UserRepo : IUserRepo
    {
        private readonly ScrowdDbContext dbContext;
        private readonly RestClient restClient;
        private readonly AppSettings appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepo"/> class.
        /// </summary>
        /// <param name="dbContext">db connection. dependecy injected.</param>
        /// <param name="appSettings">config settings of the app.</param>
        public UserRepo(ScrowdDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            this.dbContext = dbContext;
            this.appSettings = appSettings.Value;
            this.restClient = new RestClient(this.appSettings.AuthorizationServer);
        }

        /// <summary>
        /// Returns the user with given facebook user id.
        /// In the future if more user types are nedded
        /// room for improvement.
        /// </summary>
        /// <param name="userId">Facebook user id.</param>
        public async Task<User> GetFacebookUser(string userId)
        {
            return await this.dbContext.User.FirstOrDefaultAsync(u => u.FacebookUserId.Equals(userId));
        }

        /// <summary>
        /// get user by user id
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>user</returns>
        public async Task<User> GetUserById(int userId)
        {
            return await this.dbContext.User.FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }

        /// <summary>
        /// register user in identity server and register user in db.
        /// </summary>
        /// <param name="registerData">registeration data.</param>
        public async Task<User> RegisterUser(RegisterModel registerData)
        {
            RestRequest request = new RestRequest("Account/RegisterUser", Method.POST);
            request.AddJsonBody(new { UserType = (int)UserType.Facebook, Email = string.Empty, Token = registerData.AccessToken });
            // user returned by the identity server
            IdentityServerRegisterResponse registerResponse = await this.restClient.PostAsync<IdentityServerRegisterResponse>(request);
            if (registerResponse.StatusCode != 200)
            {
                // Oh ohh, error occured during registeration in identity server
                throw new ApplicationException(
                    registerResponse.StatusCode ?? (int)ApplicationErrorCode.UnknownError,
                    registerResponse.ErrorDescription ?? "Failed to register user in identity server");
            }
            User user = new User();
            user.Email = registerData.Email;
            user.FacebookUserId = registerResponse.Data.UserId;
            user.Name = registerResponse.Data.Name;
            user.UserType = (int)UserType.Facebook;
            user.ProfilePicture = registerResponse.Data.ProfilePicture;
            user.AccessToken.Token = registerData.AccessToken;
            // TODO- Profile Picture
            var result = await this.dbContext.User.AddAsync(user);
            return user;
        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// saves fcm token
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="fcmToken">fmc token</param>
        /// <returns>async task</returns>
        public async Task SaveFcmToken(int userId, string fcmToken)
        {
            FirebaseMessagingToken existingToken = await this.dbContext.FirebaseToken.FirstOrDefaultAsync(token => token.User.Id == userId);
            if (existingToken == null)
            {
                // no token yet saved, insert
                FirebaseMessagingToken newFcmToken = new FirebaseMessagingToken()
                {
                User = await this.GetUserById(userId),
                Token = fcmToken
                };
                await this.dbContext.FirebaseToken.AddAsync(newFcmToken);
            }
            else
            {
                // update
                existingToken.Token = fcmToken;
                this.dbContext.FirebaseToken.Update(existingToken);
            }
        }
    }
}