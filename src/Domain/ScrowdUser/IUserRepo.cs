namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Account;
    using SprintCrowd.BackEnd.Web.PushNotification;

    /// <summary>
    /// interface for UserRepo.
    /// </summary>
    public interface IUserRepo
    {
        /// <summary>
        /// Gets user info
        /// </summary>
        /// <param name="userId">user id for lookup</param>
        /// <returns><see cref="User"> user info details </see></returns>
        Task<User> GetUser(int userId);

        /// <summary>
        /// returned the user by facebook user id.
        /// </summary>
        /// <param name="facebookUserId">facebook user id of the user.</param>
        Task<User> GetFacebookUser(string facebookUserId);
        /// <summary>
        /// returns user by user id
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>user</returns>
        Task<User> GetUserById(int userId);
        /// <summary>
        /// register a user.
        /// </summary>
        /// <param name="registerData">registration data.</param>
        Task<User> RegisterUser(RegisterModel registerData);
        /// <summary>
        /// Register Email User
        /// </summary>
        /// <param name="emailUserData"></param>
        /// <returns></returns>
        Task<User> RegisterEmailUser(EmailUser emailUserData);
        /// <summary>
        ///Email Confirmation By Mail
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        Task<bool> EmailConfirmationByMail(EmailUser registerData);

        /// <summary>
        /// Get Sprint By Promotion Code
        /// </summary>
        /// <param name="promoCode"></param>
        /// <returns></returns>
        Task<Sprint> GetSprintByPromoCode(string promoCode);

        /// <summary>
        /// saves fcm token
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="fcmToken">fmc token</param>
        /// <returns>async task</returns>
        Task SaveFcmToken(int userId, string fcmToken);
        /// <summary>
        /// commit changes to db and save changes.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Save user activity
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>async task</returns>
        Task<UserActivity> UpdateUserActivity(int userId);

        /// <summary>
        /// Get user preference
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>user preference</returns>
        Task<UserPreference> GetUserPreference(int userId);

        /// <summary>
        /// Add user preference
        /// </summary>
        /// <param name="userId">user id to add</param>
        Task AddUserPreference(int userId);

        /// <summary>
        /// Update user preference
        /// </summary>
        /// <param name="userPreference">update user preference</param>
        void UpdateUserPreference(UserPreference userPreference);

        /// <summary>
        /// Get user settings
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>user notification reminders</returns>
        Task<UserNotificationReminder> GetUserSettings(int userId);

        /// <summary>
        /// Add User promotion code data
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="promoCode"></param>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        Task AddPromocodeUser(int userId, string promoCode, int sprintId);

        /// <summary>
        /// Add default user settings for given user id
        /// </summary>
        /// <param name="userId">user id to add</param>
        Task AddDefaultUserSettings(int userId);

        /// <summary>
        /// Update user
        /// </summary>
        void UpdateUser(User user);

        /// <summary>
        /// Update user settings for notification reminder
        /// </summary>
        void UpdateUserSettings(UserNotificationReminder notificationReminder);
    }
}