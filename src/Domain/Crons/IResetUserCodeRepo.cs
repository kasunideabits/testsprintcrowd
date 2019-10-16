namespace SprintCrowd.BackEnd.Domain.Crons
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  /// Interface for user reset code
  /// </summary>
  public interface IResetUserCodeRepo
  {
    /// <summary>
    /// commit and save changes to the db
    /// only call this from the service, DO NOT CALL FROM REPO ITSELF
    /// Unit of work methology.
    /// </summary>
    void SaveChanges();

    /// <summary>
    /// Reset code of all users
    /// </summary>
    Task<User> ResetUserCode(User user);

    /// <summary>
    /// Get all users
    /// </summary>
    Task<List<User>> GetAllUsers();

    /// <summary>
    /// Update single user
    /// </summary>
    Task<User> UpdateUserContext(User user);
  }
}