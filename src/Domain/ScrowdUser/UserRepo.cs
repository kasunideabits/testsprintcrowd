namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using RestSharp;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Models;
    using SprintCrowd.BackEnd.Web.Account;

    /// ONLY REPOSITORIES WILL ACCESS THE DATABASE
    /// NO DIRECT ACCESS FROM SERVICES OR CONTROLLERS ALLOWED.
    /// <summary>
    ///  user repository for getting and modifing users.
    /// </summary>
    public class UserRepo : IUserRepo
    {
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

        private readonly ScrowdDbContext dbContext;
        private readonly RestClient restClient;
        private readonly AppSettings appSettings;

        /// <summary>
        /// Gets user info
        /// </summary>
        /// <param name="userId">user id for lookup</param>
        /// <returns><see cref="User"> user info details </see></returns>
        public Task<User> GetUser(int userId)
        {
            return this.dbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
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

            var exist = await this.dbContext.User.FirstOrDefaultAsync(u => u.Email.Equals(registerData.Email));

            var code = SCrowdUniqueKey.GetUniqueKey();
            var codeExist = await this.dbContext.User.FirstOrDefaultAsync(u => u.Code.Equals(code));

            while (codeExist != null)
            {
                code = SCrowdUniqueKey.GetUniqueKey();
                codeExist = await this.dbContext.User.FirstOrDefaultAsync(u => u.Code.Equals(code));
            }

            if (exist == null)
            {
                User user = new User();
                user.Email = registerData.Email;
                user.FacebookUserId = registerResponse.Data.UserId;
                user.Name = registerResponse.Data.Name;
                user.UserType = (int)UserType.Facebook;
                user.ProfilePicture = registerResponse.Data.ProfilePicture;
                user.AccessToken.Token = registerData.AccessToken;
                user.Country = registerResponse.Data.Country;
                user.City = registerResponse.Data.City;
                user.CountryCode = registerResponse.Data.CountryCode;
                user.Code = code;
                user.ColorCode = new UserColorCode().PickColor();
                user.UserState = UserState.Active;
                var FbUser = await this.dbContext.User.AddAsync(user);
                return FbUser.Entity;
            }
            else
            {
                exist.UserState = UserState.Active;
                this.dbContext.Update(exist);
            }
            return exist;
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
                    Token = fcmToken,
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

        /// <summary>
        /// Save user activity
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>async task</returns>
        public async Task<UserActivity> UpdateUserActivity(int userId)
        {
            UserActivity userActivity = new UserActivity()
            {
                UserId = userId
            };
            var activity = await this.dbContext.UserActivity.AddAsync(userActivity);
            return activity.Entity;
        }

        public async Task<UserPreference> GetUserPreference(int userId)
        {
            return await this.dbContext.UserPreferences.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task AddUserPreference(int userId)
        {
            await this.dbContext.UserPreferences.AddAsync(new UserPreference() { UserId = userId });
            return;
        }

        /// <summary>
        /// Update user preference
        /// </summary>
        /// <param name="userPreference">update user preference</param>
        public void UpdateUserPreference(UserPreference userPreference)
        {
            this.dbContext.Update(userPreference);
        }

        /// <summary>
        /// Get user settings
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>user notification reminders</returns>
        public async Task<UserNotificationReminder> GetUserSettings(int userId)
        {
            return await this.dbContext.UserNotificationReminders.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == userId);
        }

        /// <summary>
        /// Add default user settings for given user id
        /// </summary>
        /// <param name="userId">user id to add</param>
        public async Task AddDefaultUserSettings(int userId)
        {
            await this.dbContext.UserNotificationReminders.AddAsync(new UserNotificationReminder() { UserId = userId });
        }

        /// <summary>
        /// Update user
        /// </summary>
        public void UpdateUser(User user)
        {
            this.dbContext.User.Update(user);
        }

        /// <summary>
        /// Update user settings for notification reminder
        /// </summary>
        public void UpdateUserSettings(UserNotificationReminder notificationReminder)
        {
            this.dbContext.UserNotificationReminders.Update(notificationReminder);
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="keyword">user id to add</param>
        /// <param name="pageIndex">page no</param>
        /// <returns><see cref="User"> All users info details </see></returns>
        public async Task<List<User>> GetAllUsers(string keyword, int pageIndex)
        {
            // var allUsers = await (from user in this.dbContext.User where (keyword.Equals("null") || user.Name.StartsWith(keyword, System.StringComparison.OrdinalIgnoreCase)) select user).Distinct().AsQueryable().ToListAsync();

            // var allUsers = await (from user in this.dbContext.User
            //                       join participant in this.dbContext.SprintParticipant on user.Id equals participant.UserId
            //                       join sprint in this.dbContext.Sprint on participant.SprintId equals sprint.Id
            //                       where (keyword.Equals("null") || user.Name.StartsWith(keyword, System.StringComparison.OrdinalIgnoreCase)
            //                       )
            //                       select user).Skip((pageIndex - 1) * 10).Take(10).ToListAsync();

            var filteredUsers = await (from user in this.dbContext.User
                                       join participant in this.dbContext.SprintParticipant on user.Id equals participant.UserId
                                       select user).ToListAsync();

            var allUsers = await (from user in this.dbContext.User where (keyword.Equals("null") || user.Name.StartsWith(keyword, System.StringComparison.OrdinalIgnoreCase)) select user).Distinct().AsQueryable().ToListAsync();

            allUsers.RemoveAll(x => filteredUsers.Contains(x));

            return allUsers.Skip((pageIndex - 1) * 100).Take(100).Distinct().AsQueryable().ToList();
        }
    }
}