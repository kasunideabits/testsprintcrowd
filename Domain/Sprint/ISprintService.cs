namespace SprintCrowdBackEnd.Domain.Sprint
{
  using System.Threading.Tasks;
  using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
  /// <summary>
  /// ISprintService interface
  /// </summary>
  public interface ISprintService
  {
    /// <summary>
    /// Create a new sprint
    /// </summary>
    /// <param name="sprintInfo">sprint information</param>
    /// /// <param name="ownerOfSprint">user who creatse the sprint</param>
    /// <returns>cereated sprint</returns>
    Task<Sprint> CreateNewSprint(SprintModel sprintInfo, User ownerOfSprint);
  }
}