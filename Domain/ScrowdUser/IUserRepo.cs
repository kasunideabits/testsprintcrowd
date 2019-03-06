namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Web.Account;
    using SprintCrowdBackEnd.Web.PushNotification;

    /// <summary>
    /// interface for UserRepo.
    /// </summary>
    public interface IUserRepo
    {
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
    }
}