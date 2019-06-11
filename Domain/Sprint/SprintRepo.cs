namespace SprintCrowd.BackEnd.Domain.Sprint
{
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  /// <summary>
  /// Event repositiory
  /// </summary>
  public class SprintRepo : ISprintRepo
  {
    private ScrowdDbContext dbContext;
    /// <summary>
    /// intializes an instace of EventRepo
    /// </summary>
    /// <param name="dbContext">db context</param>
    public SprintRepo(ScrowdDbContext dbContext)
    {
      this.dbContext = dbContext;
    }
    /// <summary>
    /// creates event
    /// </summary>
    /// <param name="SprintToAdd">event model</param>
    /// <returns></returns>
    public async Task<Sprint> AddSprint(Sprint SprintToAdd)
    {
      var result = await this.dbContext.Sprint.AddAsync(SprintToAdd);
      return result.Entity;
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
  }
}