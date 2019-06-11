namespace SprintCrowd.BackEnd.Domain.ScrowdUser
{
  using System.Threading.Tasks;
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
  }
}