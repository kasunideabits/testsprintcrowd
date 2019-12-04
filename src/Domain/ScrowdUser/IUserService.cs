namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Web.Account;
  using SprintCrowd.BackEnd.Web.PushNotification;
  using SprintCrowd.BackEnd.Web.ScrowdUser.Models;

  /// <summary>
  /// interface for UserService.
  /// </summary>
  public interface IUserService
  {
    /// <summary>
    /// Gets user info
    /// </summary>
    /// <param name="userId">user id for lookup</param>
    /// <returns><see cref="UserDto"> user info details </see></returns>
    Task<UserDto> GetUser(int? userId);

    /// <summary>
    /// retrieve the user by facebook user id.
    /// </summary>
    /// <param name="facebookUserId">facebook user id of the user.</param>
    Task<User> GetFacebookUser(string facebookUserId);

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

    /// <summary>
    /// Save user activity
    /// </summary>
    /// <param name="userId">id of the user</param>
    /// <returns>async task</returns>
    Task<UserActivity> UpdateUserActivity(int userId);

    /// <summary>
    ///  Get user preference
    /// </summary>
    /// <param name="userId">user id to fetch preference</param>
    /// <returns>user preference</returns>
    Task<UserPreferenceDto> GetUserPreference(int userId);

    /// <summary>
    /// Update user preferences
    /// </summary>
    /// <param name="userId"> user id to for update user </param>
    /// <param name="userPreferenceModel">user preference</param>
    /// <returns>updated user preference</returns>
    Task<UserPreferenceDto> UpdateUserPreference(int userId, UserPreferenceModel userPreferenceModel);
  }
}