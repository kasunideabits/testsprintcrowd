namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Account;
    using SprintCrowd.BackEnd.Web.PushNotification;
    using SprintCrowd.BackEnd.Web.ScrowdUser.Models;

    /// <summary>
    /// user service used for managing users.
    /// Authorization dosent happen here.
    /// Auth happens in identity server.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepo">instance of userRepo, dependency injected.</param>

        public UserService(IUserRepo userRepo, ISprintParticipantService _sprintParticipantService)
        {
            this.userRepo = userRepo;
            this.sprintParticipantService = _sprintParticipantService;
        }

        private ISprintParticipantService sprintParticipantService { get; }

        /// <summary>
        /// Gets user info
        /// </summary>
        /// <param name="userId">user id for lookup</param>
        /// <returns><see cref="UserDto"> user info details </see></returns>
        public async Task<UserDto> GetUser(int? userId)
        {
            if (userId != null)
            {
                var user = await this.userRepo.GetUser((int)userId);
                return new UserDto(user.Id, user.Name, user.ProfilePicture, user.Code);
            }
            else
            {
                throw new Application.ApplicationException("User Not Found");
            }
        }

        /// <summary>
        /// Get facebook User.
        /// </summary>
        /// <param name="userId">Facebook user id.</param>
        public async Task<User> GetFacebookUser(string userId)
        {
            var result = await this.userRepo.GetFacebookUser(userId);
            if (result.UserState != Application.UserState.Active)
            {
                result.UserState = Application.UserState.Active;
                this.userRepo.UpdateUser(result);
                this.userRepo.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// register a user
        /// if multiple user types are needed, facebook, google etc
        /// then heres the place to make the change.
        /// </summary>
        /// <param name="registerData">registeration data.</param>
        public async Task<User> RegisterUser(RegisterModel registerData)
        {
            User user = await this.userRepo.RegisterUser(registerData);
            await this.userRepo.AddUserPreference(user.Id);
            await this.userRepo.AddDefaultUserSettings(user.Id);
            this.userRepo.SaveChanges();
            return user;
        }

        /// <summary>
        /// register a email user
        /// if multiple user types are needed
        /// then heres the place to make the change.
        /// </summary>
        /// <param name="registerData">registeration data.</param>
        public async Task<User> RegisterEmailUser(EmailUser registerData)
        {
            User user = await this.userRepo.RegisterEmailUser(registerData);
            await this.userRepo.AddUserPreference(user.Id);
            await this.userRepo.AddDefaultUserSettings(user.Id);
           // await this.userRepo.AddPromocodeUser(user.Id, registerData.PromotionCode, registerData.SprintId);
            this.userRepo.SaveChanges();
            //Promocode User join to the sprint
           
            return user;
        }

        /// <summary>
        /// Add Promotion Code
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<Sprint> AddPromotionCode(EmailUser registerData, int userId)
        {
            //Get the sprint Id related to promotion code.
            Sprint sprint = await this.userRepo.GetSprintByPromoCode(registerData.PromotionCode);

            if (sprint != null)
            {
                ///Add Promotion code user details.
                await this.userRepo.AddPromocodeUser(userId, registerData.PromotionCode, sprint.Id);
                this.userRepo.SaveChanges();
                //join to sprint after adding promotion code
                await this.sprintParticipantService.JoinSprint(
                  sprint.Id,
                  userId,
                  0,
                  true
              );
            }
            else
            {
                throw new Application.SCApplicationException((int)ErrorCodes.InvalidPromotionCode, "Promotion code is invalid or expired.");
            }

            return sprint;
        }

        /// <summary>
        /// Email Confirmation By Mail
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> EmailConfirmationByMail(EmailUser registerData)
        {
            bool success = await this.userRepo.EmailConfirmationByMail(registerData);
            //await this.userRepo.AddUserPreference(user.Id);
            //await this.userRepo.AddDefaultUserSettings(user.Id);
            //this.userRepo.SaveChanges();
            return success;
        }

        /// <summary>
        /// Generate Email User Token For Password Reset
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> GenerateEmailUserTokenForPwReset(EmailUser registerData)
        {
            bool success = await this.userRepo.GenerateEmailUserTokenForPwReset(registerData);
            return success;
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        public async Task<bool> ResetPassword(EmailUser registerData)
        {
            bool success = await this.userRepo.ResetPassword(registerData);
            return success;
        }

        /// <summary>
        /// saves fcm token
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="fcmToken">fcm token</param>
        /// <returns>async task</returns>
        public async Task SaveFcmToken(int userId, string fcmToken)
        {
            await this.userRepo.SaveFcmToken(userId, fcmToken);
            this.userRepo.SaveChanges();
        }

        /// <summary>
        /// Save user activity
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>async task</returns>
        public async Task<UserActivity> UpdateUserActivity(int userId)
        {
            UserActivity userActivity = await this.userRepo.UpdateUserActivity(userId);
            this.userRepo.SaveChanges();
            return userActivity;
        }

        /// <summary>
        /// Get user preference
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>user preference</returns>
        public async Task<UserPreferenceDto> GetUserPreference(int userId)
        {
            var userPreference = await this.userRepo.GetUserPreference(userId);
            if (userPreference == null)
            {
                throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User Preference not found");
            }

            return new UserPreferenceDto(userPreference);
        }

        /// <summary>
        /// Update user preferences
        /// </summary>
        /// <param name="userId"> user id to for update user </param>
        /// <param name="userPreferenceModel">user preference</param>
        /// <returns>updated user preference</returns>
        public async Task<UserPreferenceDto> UpdateUserPreference(int userId, UserPreferenceModel userPreferenceModel)
        {
            var userPreference = await this.userRepo.GetUserPreference(userId);
            if (userPreference == null)
            {
                throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User Preference not found");
            }
            userPreference.Mon = userPreferenceModel.Day.Mon;
            userPreference.Tue = userPreferenceModel.Day.Tue;
            userPreference.Wed = userPreferenceModel.Day.Wed;
            userPreference.Thur = userPreferenceModel.Day.Thur;
            userPreference.Fri = userPreferenceModel.Day.Fri;
            userPreference.Sat = userPreferenceModel.Day.Sat;
            userPreference.Sun = userPreferenceModel.Day.Sun;
            userPreference.Morning = userPreferenceModel.Time.Morning;
            userPreference.AfterNoon = userPreferenceModel.Time.AfterNoon;
            userPreference.Evening = userPreferenceModel.Time.Evening;
            userPreference.Night = userPreferenceModel.Time.Night;
            userPreference.TwoToFive = userPreferenceModel.Distance.TwoToFive;
            userPreference.SixToTen = userPreferenceModel.Distance.SixToTen;
            userPreference.ElevenToFifteen = userPreferenceModel.Distance.ElevenToFifteen;
            userPreference.SixteenToTwenty = userPreferenceModel.Distance.SixteenToTwenty;
            userPreference.TOneToThirty = userPreferenceModel.Distance.TOneToThirty;
            userPreference.ThirtyOneToFortyOne = userPreferenceModel.Distance.ThirtyOneToFortyOne;
            this.userRepo.UpdateUserPreference(userPreference);
            this.userRepo.SaveChanges();
            return new UserPreferenceDto(userPreference);
        }

        /// <summary>
        /// Get user settings with given user id
        /// </summary>
        /// <param name="userId">user id to fetch settings</param>
        /// <returns>user settings</returns>
        public async Task<UserSettingsDto> GetUserSettings(int userId)
        {
            var userSettings = await this.userRepo.GetUserSettings(userId);
            if (userSettings == null)
            {
                throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User settings not found");
            }
            return new UserSettingsDto(userSettings.User.LanguagePreference, userSettings);
        }

        /// <summary>
        /// Update user settings
        /// </summary>
        /// <param name="userId"> user id to for update user </param>
        /// <param name="userSettingsModel">user settings</param>
        /// <returns>updated user settings</returns>
        public async Task<UserSettingsDto> UpdateUserSettings(int userId, UserSettingsModel userSettingsModel)
        {
            var userSettings = await this.userRepo.GetUserSettings(userId);
            if (userSettings == null)
            {
                throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User settings not found");
            }
            userSettings.TwentyFourH = userSettingsModel.Reminder.TwentyForH;
            userSettings.OneH = userSettingsModel.Reminder.OneH;
            userSettings.FiftyM = userSettingsModel.Reminder.FiftyM;
            userSettings.EventStart = userSettingsModel.Reminder.EventStart;
            userSettings.FinalCall = userSettingsModel.Reminder.FinalCall;
            this.userRepo.UpdateUserSettings(userSettings);
            var user = await this.userRepo.GetUser(userId);
            if (user.LanguagePreference != userSettingsModel.Language)
            {
                user.LanguagePreference = userSettingsModel.Language;
                this.userRepo.UpdateUser(user);
            }
            this.userRepo.SaveChanges();
            return new UserSettingsDto(user.LanguagePreference, userSettings);
        }

        /// <summary>
        /// Deactivate account
        /// </summary>
        /// <param name="userId">user id to deactivate</param>
        public async Task AccountDeactivate(int userId)
        {
            var user = await this.userRepo.GetUser(userId);
            user.UserState = Application.UserState.Deactivate;
            this.userRepo.UpdateUser(user);
            this.userRepo.SaveChanges();
        }

        /// <summary>
        /// Logout user account
        /// </summary>
        /// <param name="userId">user id to logout</param>
        public async Task AccountLogout(int userId)
        {
            var user = await this.userRepo.GetUser(userId);
            user.UserState = Application.UserState.Logout;
            this.userRepo.UpdateUser(user);
            this.userRepo.SaveChanges();
        }

        /// <summary>
        /// Is User Exist In C
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsUserExistInSC(string email)
        {
            bool success = await this.userRepo.IsUserExistInSC(email);
            return success;
        }
    }
}