using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestSharp;
using SprintCrowd.Backend.Application;
using SprintCrowd.Backend.Models;
using SprintCrowdBackEnd.Application;
using SprintCrowdBackEnd.Infrastructure.Persistence;
using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
using SprintCrowdBackEnd.Web.Account;

namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    /// ONLY REPOSITORIES WILL ACCESS THE DATABASE
    /// NO DIRECT ACCESS FROM SERVICES OR CONTROLLERS ALLOWED
    /// <summary>
    ///  user repository for getting and modifing users
    /// </summary>
    public class UserRepo : IUserRepo
    {
        private readonly ScrowdDbContext dbContext;
        private readonly RestClient restClient;
        private readonly AppSettings appSettings;
        public UserRepo(ScrowdDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            this.dbContext = dbContext;
            this.appSettings = appSettings.Value;
            this.restClient = new RestClient(this.appSettings.AuthorizationServer);
        }

        /// <summary>
        /// Returns the user with given facebook user id.
        /// In the future if more user types are nedded
        /// room for improvement
        /// </summary>
        /// <param name="userId">Facebook user id</param>
        public async Task<User> GetUser(string userId)
        {
            return await dbContext.User.FirstOrDefaultAsync(u => u.FacebookUserId.Equals(userId));
        }

        /// <summary>
        /// register user in identity server and register user in db
        /// </summary>
        /// <param name="registerData">registeration data</param>
        public async Task<User> RegisterUser(RegisterModel registerData)
        {
            RestRequest request = new RestRequest("Account/RegisterUser", Method.POST);
            request.AddJsonBody(new {
                UserType = (int)UserType.Facebook,
                Email = "", // not needed email as identity server only care about user id for facebook user which it will retrieve using access token
                Token = registerData.AccessToken
            });
            // user returned by the identity server
            IdentityServerRegisterResponse registerResponse = await this.restClient.PostAsync<IdentityServerRegisterResponse>(request);
            if(registerResponse.StatusCode != 200)
            {
                //Oh ohh, error occured during registeration in identity server
                throw new ApplicationException(registerResponse.StatusCode ?? (int)ApplicationErrorCodes.UnknownError,
                                               registerResponse.ErrorDescription ?? "Failed to register user in identity server");
            }
            User user = new User();
            user.Email = registerData.Email;
            user.FacebookUserId = registerResponse.Data.UserId;
            user.Name = registerResponse.Data.Name;
            //TODO- Profile Picture
            var result = await dbContext.User.AddAsync(user);
            return user;
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}