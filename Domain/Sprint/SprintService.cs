namespace SprintCrowd.BackEnd.Domain.Sprint
{

  using System.Threading.Tasks;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  ///   /// Sprint service
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
      sprint_avail.Type = sprintData.SprintType;
      sprint_avail.LocationProvided = sprintData.LocationProvided;
      sprint_avail.Lattitude = sprintData.Lattitude;
      sprint_avail.Longitutude = sprintData.Longitutude;

      var value = sprint_avail.Id;
      if (sprint_avail != null)
      {
        Sprint sprint = await this.SprintRepo.UpdateSprint(sprint_avail);

        if (sprint != null)
        {
          this.SprintRepo.SaveChanges();

        }
        return sprint;
      }

      return null;
    }
    /// <summary>
    /// creates a new sprint
    /// </summary>
    /// <param name="sprintInfo">info about the sprint</param>
    /// /// <param name="ownerOfSprint">user who created the sprint</param>
    /// <returns>created sprint</returns>
    public async Task<Sprint> CreateNewSprint(SprintModel sprintInfo, User ownerOfSprint)
    {
      Sprint sprintToBeCreated = new Sprint();
      sprintToBeCreated.CreatedBy = ownerOfSprint;
      sprintToBeCreated.Type = sprintInfo.SprintType;
      sprintToBeCreated.LocationProvided = sprintInfo.LocationProvided;
      sprintToBeCreated.Lattitude = sprintInfo.Lattitude;
      sprintToBeCreated.Longitutude = sprintInfo.Longitutude;
      sprintToBeCreated.Name = sprintInfo.Name;
      sprintToBeCreated.StartDateTime = sprintInfo.StartTime;
      sprintToBeCreated.Status = (int)SprintStatus.NOTSTARTEDYET;
      sprintToBeCreated.Distance = sprintInfo.Distance;

      Sprint sprint = await this.SprintRepo.AddSprint(sprintToBeCreated);
      if (sprint != null)
      {
        this.SprintRepo.SaveChanges();
      }

      return sprint;
    }
  }
}