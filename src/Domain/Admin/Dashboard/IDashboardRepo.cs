namespace SprintCrowd.BackEnd.Domain.Admin.Dashboard
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Domain.Device;

  /// <summary>
  /// Interface for handle friend
  /// </summary>
  public interface IDashboardRepo
  {
    /// <summary>
    /// commit and save changes to the db
    /// only call this from the service, DO NOT CALL FROM REPO ITSELF
    /// Unit of work methology.
    /// </summary>
    void SaveChanges();

    /// <summary>
    /// Get weekly active users
    /// </summary>
    /// <returns>Number of weekly active users</returns>
    int GetWeeklyActiveUsers();
  }
}