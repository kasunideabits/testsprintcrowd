namespace SprintCrowd.BackEnd.Domain.Sprint
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  /// <summary>
  /// ISprintService interface
  /// </summary>
  public interface ISprintService
  {
    /// <summary>
    /// Get all events
    /// </summary>
    /// <returns>Available events</returns>
    Task<List<Sprint>> GetAll(int eventType);

    /// <summary>
    /// Create a new sprint
    /// </summary>
    /// <param name="sprintInfo">sprint information</param>
    /// /// <param name="ownerOfSprint">user who creatse the sprint</param>
    /// <returns>cereated sprint</returns>
    Task<Sprint> CreateNewSprint(SprintModel sprintInfo, User ownerOfSprint);
    /// <summary>
    /// update sprint
    /// </summary>
    /// <param name="sprintData">sprint information</param>
    /// <returns></returns>

    Task<Sprint> UpdateSprint(SprintModel sprintData);

  }
}