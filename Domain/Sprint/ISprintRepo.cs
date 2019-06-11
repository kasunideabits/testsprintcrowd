namespace SprintCrowdBackEnd.Domain.Sprint
{
  using System.Threading.Tasks;
  using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
  /// <summary>
  /// inerface for event repo
  /// </summary>
  public interface ISprintRepo
  {
    /// <summary>
    /// adds new event to database
    /// </summary>
    /// <param name="EventToCreate">event model</param>
    /// <returns></returns>
    Task<Sprint> AddSprint(Sprint EventToCreate);
    /// <summary>
    /// Updates event in database
    /// </summary>
    /// <param name="SprintData">event model</param>
    Task<Sprint> UpdateSprint(Sprint SprintData);
    /// <summary>
    /// Check event in database
    /// </summary>
    /// <param name="SprintID">event model</param>
    Task<Sprint> GetSprint(int SprintID);

    /// <summary>
    /// saves changed to db
    /// </summary>
    void SaveChanges();
  }
}