namespace SprintCrowd.BackEnd.Domain.Admin.Dashboard
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Common;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  ///  Interface for sprint crowd frined service
  /// </summary>
  public interface IDashboardService
  {
    /// <summary>
    /// Get dashboard related data
    /// </summary>
    DashboardDataDto GetDashboardData();
  }
}