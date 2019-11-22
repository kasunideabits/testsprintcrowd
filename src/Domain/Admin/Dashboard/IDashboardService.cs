namespace SprintCrowd.BackEnd.Domain.Admin.Dashboard
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Common;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Domain.Sprint;
  using SprintCrowd.BackEnd.Domain.Device;

  /// <summary>
  ///  Interface for sprint crowd frined service
  /// </summary>
  public interface IDashboardService
  {
    /// <summary>
    /// Get dashboard related data
    /// </summary>
    DashboardDataDto GetDashboardData(LiveSprintCount liveSprintsCount, DeviceModal appdownloads);
  }
}