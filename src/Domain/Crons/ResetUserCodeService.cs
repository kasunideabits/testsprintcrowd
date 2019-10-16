namespace SprintCrowd.BackEnd.Domain.Crons
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Application;

  /// <summary>
  ///  Implement <see cref="IResetUserCodeService" > interface </see>
  /// </summary>
  public class ResetUserCodeService : IResetUserCodeService
  {
    /// <summary>
    /// Initialize <see cref="ResetUserCodeService"> class </see>
    /// </summary>
    /// <param name="resetUserCodeRepo">friend repository</param>
    public ResetUserCodeService(IResetUserCodeRepo resetUserCodeRepo)
    {
      this.ResetUserCodeRepo = resetUserCodeRepo;
    }

    private IResetUserCodeRepo ResetUserCodeRepo { get; }

    /// <summary>
    /// Reset code of all users
    /// </summary>
    public async Task ResetUserCode()
    {
      var users = await this.ResetUserCodeRepo.GetAllUsers();

      users.ForEach(async user =>
      {
        var updatedUser = await this.ResetUserCodeRepo.ResetUserCode(user);
        var saveUserInstance = await this.ResetUserCodeRepo.UpdateUserContext(updatedUser);
      });
      this.ResetUserCodeRepo.SaveChanges();
    }
  }
}