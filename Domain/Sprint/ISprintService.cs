namespace SprintCrowd.BackEnd.Domain.Sprint
{
  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
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

    Task<Sprint> UpdateSprint(SprintModel sprintData);
  }
}