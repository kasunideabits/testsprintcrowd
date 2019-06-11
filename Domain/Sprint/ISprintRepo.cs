namespace SprintCrowd.BackEnd.Domain.Sprint
{
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
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
    /// saves changed to db
    /// </summary>
    void SaveChanges();
  }
}