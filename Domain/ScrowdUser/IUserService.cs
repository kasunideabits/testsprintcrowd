namespace SprintCrowdBackEnd.Domain.ScrowdUser
{
    using System.Threading.Tasks;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Web.Account;
    using SprintCrowdBackEnd.Web.PushNotification;

    /// <summary>
    /// interface for UserService.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// retrieve the user by facebook user id.
        /// </summary>
        /// <param name="userId">facebook user id of the user.</param>
        Task<User> GetFacebookUser(string userId);
        /// <summary>
        /// register a user.
        /// </summary>
        /// <param name="registerData">register data of the user you want to register.</param>
        Task<User> RegisterUser(RegisterModel registerData);
        /// <summary>
        /// saves fcm token
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <param name="fcmToken">fmc token</param>
        /// <returns>async task</returns>
        Task SaveFcmToken(int userId, string fcmToken);
    }
}