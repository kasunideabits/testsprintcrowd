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
  }
}