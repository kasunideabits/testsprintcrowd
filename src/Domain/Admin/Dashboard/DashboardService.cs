using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Admin.Dashboard
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using System;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  ///  Implement <see cref="IDashboardService" > interface </see>
  /// </summary>
  public class DashboardService : IDashboardService
  {
    /// <summary>
    /// Initialize <see cref="DashboardService"> class </see>
    /// </summary>
    /// <param name="DashboardRepo">friend repository</param>
    public DashboardService(IDashboardRepo DashboardRepo)
    {
      this.DashboardRepo = DashboardRepo;
    }

    private IDashboardRepo DashboardRepo { get; }

    /// <summary>
    /// Get dashboard related data
    /// </summary>
    public DashboardDataDto GetDashboardData()
    {
      DashboardDataDto dashboardData = new DashboardDataDto()
      {
        WeeklyActiveUsers = this.DashboardRepo.GetWeeklyActiveUsers()
      };

      return dashboardData;
    }
  }
}