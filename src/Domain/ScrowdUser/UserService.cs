namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Web.Account;
  using SprintCrowd.BackEnd.Web.PushNotification;

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

    public UserService(IUserRepo userRepo)
    {
      this.userRepo = userRepo;
    }

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
      return await this.userRepo.GetFacebookUser(userId);
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
      this.userRepo.SaveChanges();
      return user;
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
        throw new Application.SCApplicationException((int)UserErrorCode.UserNotFound, "User Not found");
      }

      return new UserPreferenceDto(userPreference);
    }

  }
}