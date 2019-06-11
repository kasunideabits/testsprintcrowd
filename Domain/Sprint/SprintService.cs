namespace SprintCrowdBackEnd.Domain.Sprint
{

  using System.Threading.Tasks;
  using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  /// Sprint service
  /// </summary>
  public class SprintService : ISprintService
  {
    private ISprintRepo SprintRepo;
    /// <summary>
    /// initializes an instance of SprintService
    /// </summary>
    /// <param name="sprintRepo">sprint repository</param>
    public SprintService(ISprintRepo sprintRepo)
    {
      this.SprintRepo = sprintRepo;
    }
    /// <summary>
    /// Update instance of SprintService
    /// </summary>
    /// <param name="sprintData">sprint repository</param>
    public async Task<Sprint> UpdateSprint(SprintModel sprintData)
    {

      Sprint UpdateSprint = new Sprint();
      UpdateSprint.Id = sprintData.Id;

      var sprint_avail = await this.SprintRepo.GetSprint(UpdateSprint.Id);
      sprint_avail.Name = sprintData.Name;
      sprint_avail.Distance = sprintData.Distance;
      sprint_avail.StartDateTime = sprintData.StartTime;
      sprint_avail.Type = sprintData.EventType;
      sprint_avail.LocationProvided = sprintData.LocationProvided;
      sprint_avail.Lattitude = sprintData.Lattitude;
      sprint_avail.Longitutude = sprintData.Longitutude;

      var value = sprint_avail.Id;
      if (sprint_avail != null)
      {
        Sprint sprint = await this.SprintRepo.UpdateSprint(sprint_avail);

        this.SprintRepo.SaveChanges();
        return sprint;
      }

      return null;
    }
  }
}