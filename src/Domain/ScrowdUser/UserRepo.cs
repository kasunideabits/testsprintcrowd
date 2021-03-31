namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using RestSharp;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Models;
    using SprintCrowd.BackEnd.Web.Account;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using System.Collections.Generic;
    using System.Linq;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;
    using System.Text.RegularExpressions;


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

            if (registerData.UserType == (int)UserType.Facebook)
                request.AddJsonBody(new { UserType = (int)UserType.Facebook, Email = string.Empty, Token = registerData.AccessToken });
            else
                if (registerData.UserType == (int)UserType.AppleUser)
                request.AddJsonBody(new { UserType = (int)UserType.AppleUser, Email = registerData.Email, Token = registerData.AccessToken, Name = registerData.Name });

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
                this.dbContext.SaveChanges();
                return FbUser.Entity;
            }
            else
            {
                exist.UserState = UserState.Active;
                this.dbContext.Update(exist);
                this.dbContext.SaveChanges();
                
            }
            return exist;
        }

        /// <summary>
        /// register user in identity server and register user in db.
        /// </summary>
        /// <param name="registerData">registeration data.</param>
        public async Task<User> RegisterEmailUser(EmailUser emailUserData)
        {
            RestRequest request = new RestRequest("Account/RegisterEmailUser", Method.POST);
            request.AddJsonBody(new { Name = emailUserData.Name, Email = emailUserData.Email ,Password =emailUserData.Password});

            // user returned by the identity server
            IdentityServerRegisterResponse registerResponse = await this.restClient.PostAsync<IdentityServerRegisterResponse>(request);
            if (registerResponse.StatusCode != 200)
            {
                // Oh ohh, error occured during registeration in identity server
                throw new ApplicationException(
                    registerResponse.StatusCode ?? (int)ApplicationErrorCode.UnknownError,
                    registerResponse.ErrorDescription ?? "Failed to register user");
            }

            var exist = await this.dbContext.User.FirstOrDefaultAsync(u => u.Email.Equals(registerResponse.Data.Email));

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
                user.Email = registerResponse.Data.Email;
                user.FacebookUserId = registerResponse.Data.UserId;
                user.Name = registerResponse.Data.Name;
                user.UserType = (int)UserType.EmailUser;
                user.ProfilePicture = registerResponse.Data.ProfilePicture;
                user.AccessToken.Token = emailUserData.AccessToken;
                user.Country = registerResponse.Data.Country;
                user.City = registerResponse.Data.City;
                user.CountryCode = registerResponse.Data.CountryCode;
                user.Code = code;
                user.ColorCode = new UserColorCode().PickColor();
                user.UserState = UserState.Active;
                var FbUser = await this.dbContext.User.AddAsync(user);
                this.dbContext.SaveChanges();
                return FbUser.Entity;
            }
            else
            {
                exist.UserState = UserState.Active;
                this.dbContext.Update(exist);
                this.dbContext.SaveChanges();

            }
            return exist;
        }
        /// <summary>
        /// Email Confirmation By Mail
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> EmailConfirmationByMail(EmailUser registerData)
        {
            bool isMailSent = false;
            RestRequest request = new RestRequest("Account/EmailConfirmationByMail", Method.POST);
            request.AddJsonBody(new { Email = registerData.Email, Name = registerData.Name, Password = registerData.Password });
           
            // user returned by the identity server
            IdentityServerRegisterResponse registerResponse = await this.restClient.PostAsync<IdentityServerRegisterResponse>(request);
            if (registerResponse.StatusCode != 200)
            {
                isMailSent = false;
                throw new ApplicationException(
                    registerResponse.StatusCode ?? (int)ApplicationErrorCode.UnknownError,
                    registerResponse.ErrorDescription ?? "failed to send email confirmation fron identity");
            }
            else
                isMailSent = true;

            return isMailSent;
        }

        /// <summary>
        /// Generate Email User Token For Password Reset
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> GenerateEmailUserTokenForPwReset(EmailUser registerData)
        {
            bool isMailSent = false;
            RestRequest request = new RestRequest("Account/GenerateEmailUserTokenForPwReset", Method.POST);
            request.AddJsonBody(new { Email = registerData.Email });

            // user returned by the identity server
            IdentityServerRegisterResponse registerResponse = await this.restClient.PostAsync<IdentityServerRegisterResponse>(request);
            if (registerResponse.StatusCode != 200)
            {
                isMailSent = false;
                throw new ApplicationException(
                    registerResponse.StatusCode ?? (int)ApplicationErrorCode.UnknownError,
                    registerResponse.ErrorDescription ?? "failed to send password verification mail from identity");
            }
            else
                isMailSent = true;

            return isMailSent;
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> ResetPassword(EmailUser registerData)
        {
            bool isMailSent = false;
            RestRequest request = new RestRequest("Account/ResetPassword", Method.POST);
            request.AddJsonBody(new { Email = registerData.Email, VerificationCode = registerData.VerificationCode, Password = registerData.Password });

            // user returned by the identity server
            IdentityServerRegisterResponse registerResponse = await this.restClient.PostAsync<IdentityServerRegisterResponse>(request);
            if (registerResponse.StatusCode != 200)
            {
                isMailSent = false;
                throw new ApplicationException(
                    registerResponse.StatusCode ?? (int)ApplicationErrorCode.UnknownError,
                    registerResponse.ErrorDescription ?? "failed to reset password from identity");
            }
            else
                isMailSent = true;

            return isMailSent;
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
                this.dbContext.SaveChanges();
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

        public async Task<PromoCodeUser> GetUserSprintPromotionCode(int userId, string promoCode, int sprintId)
        {
            return await this.dbContext.PromoCodeUser.FirstOrDefaultAsync(u => u.UserId == userId && u.SprintId == sprintId && u.PromoCode == promoCode);
        }

        /// <summary>
        /// Get Sprint By Promo Code
        /// </summary>
        /// <param name="promoCode"></param>
        /// <returns></returns>
        public async Task<Sprint> GetSprintByPromoCode(string promoCode)
        {
            return await this.dbContext.Sprint.FirstOrDefaultAsync(u => u.PromotionCode == promoCode && u.StartDateTime >= System.DateTime.UtcNow);
        }

        /// <summary>
        /// Get Sprint By Promo Code
        /// </summary>
        /// <param name="promoCode"></param>
        /// <returns></returns>
        public async Task<Sprint> IsPromoCodeExist(string promoCode)
        {
            return await this.dbContext.Sprint.FirstOrDefaultAsync(u => u.PromotionCode == promoCode);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public async Task<List<UserMailReportDto>> GetAllEmailUsers()
        {
            try
            {
                List<UserMailReportDto> users = new List<UserMailReportDto>();
                var usersList = await this.dbContext.User.Select(u => new { u.Name, u.Country, u.CountryCode, u.Email }).ToListAsync();
                foreach (var user in usersList)
                {
                    var rptItem = new UserMailReportDto()
                    {
                        Name = user.Name,
                        Country = user.Country,
                        CountryCode = user.CountryCode,
                        Email = this.IsBase64String(user.Email)
                    };
                    users.Add(rptItem);
                }
                return users;
            }catch(System.Exception ex)
            {
                throw ex;
            }
        }

        public string IsBase64String(string base64)
        {
            string email = string.Empty;
            base64 = base64.Trim();
            if ((base64.Length % 4 == 0) && System.Text.RegularExpressions.Regex.IsMatch(base64, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
                email = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(base64);
            else
                email = base64;
            return email;
        }

        public async Task AddUserPreference(int userId)
        {
            var userPref = this.GetUserPreference(userId);
            if(userPref.Result ==null )
            await this.dbContext.UserPreferences.AddAsync(new UserPreference() { UserId = userId });
            return;
        }


        public async Task AddPromocodeUser(int userId , string promoCode,int sprintId )
        {
            var userPromo = this.GetUserSprintPromotionCode(userId , promoCode , sprintId);
            if (userPromo.Result == null)
                await this.dbContext.PromoCodeUser.AddAsync(new PromoCodeUser() { UserId = userId , PromoCode=promoCode,SprintId=sprintId,CreatedDate = System.DateTime.UtcNow });
            else
            {
                throw new Application.SCApplicationException((int)ErrorCodes.AlreadyJoined, "Already joined for an event");
            }
        }

        /// <summary>
        /// Update user preference
        /// </summary>
        /// <param name="userPreference">update user preference</param>
        public void UpdateUserPreference(UserPreference userPreference)
        {
            this.dbContext.Update(userPreference);
            this.dbContext.SaveChanges();
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
        /// get user by user id
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>user</returns>
        public async Task<UserNotificationReminder> GetUserNotificationReminderById(int userId)
        {
            return await this.dbContext.UserNotificationReminders.FirstOrDefaultAsync(u => u.UserId.Equals(userId));
        }

        /// <summary>
        /// Add default user settings for given user id
        /// </summary>
        /// <param name="userId">user id to add</param>
        public async Task AddDefaultUserSettings(int userId)
        {
            var userNotRem = this.GetUserNotificationReminderById(userId);
            if (userNotRem.Result == null )
            await this.dbContext.UserNotificationReminders.AddAsync(new UserNotificationReminder() { UserId = userId });
        }

        /// <summary>
        /// Update user
        /// </summary>
        public void UpdateUser(User user)
        {
            this.dbContext.User.Update(user);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Update user settings for notification reminder
        /// </summary>
        public void UpdateUserSettings(UserNotificationReminder notificationReminder)
        {
            this.dbContext.UserNotificationReminders.Update(notificationReminder);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Is User Exist In SC
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsUserExistInSC(string email)
        {
            User user = null;
            user =  await this.dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                user = await this.dbContext.User.FirstOrDefaultAsync(u => u.Email == Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(email));
           
            return (user == null)?false :true;
        }
    }
}