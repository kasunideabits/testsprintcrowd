namespace SprintCrowd.BackEnd.Domain.Admin.Dashboard
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using System;
  using Microsoft.EntityFrameworkCore;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Infrastructure.Persistence;

  /// <summary>
  /// Implement <see cref="IDashboardRepo"> repo </see>
  /// </summary>
  public class DashboardRepo : IDashboardRepo
  {
    private ScrowdDbContext dbContext;
    /// <summary>
    /// Initialize <see cref="DashboardRepo"> class </see>
    /// </summary>
    /// <param name="dbContext">database context</param>
    public DashboardRepo(ScrowdDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    /// <summary>
    /// commit and save changes to the db
    /// only call this from the service, DO NOT CALL FROM REPO ITSELF
    /// Unit of work methology.
    /// </summary>
    public void SaveChanges()
    {
      this.dbContext.SaveChanges();
    }

    /// <summary>
    /// Get weekly active users
    /// </summary>
    /// <returns>Number of weekly active users</returns>
    public int GetWeeklyActiveUsers()
    {
      var pastDate = DateTime.Now.AddDays(-7);
      var data = this.dbContext.UserActivity.Where(s => s.CreatedDate > pastDate).ToList();
      return data.Count();
    }
  }
}