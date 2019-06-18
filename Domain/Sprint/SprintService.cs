namespace SprintCrowd.BackEnd.Domain.Sprint
{
  using System.Collections.Generic;
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
    /// Get all events
    /// </summary>
    /// <returns>Available all events</returns>
    public async Task<List<Sprint>> GetAll(int eventType)
    {
      return await this.SprintRepo.GetAllEvents(eventType);
    }

    /// <summary>
    /// Update instance of SprintService
    /// </summary>
    /// <param name="sprintData">sprint repository</param>
    public async Task<Sprint> UpdateSprint(SprintModel sprintData)
    {

      Sprint UpdateSprint = new Sprint();
      UpdateSprint.Id = sprintData.Id;

      var sprintAavail = await this.SprintRepo.GetSprint(UpdateSprint.Id);
      sprintAavail.Name = sprintData.Name;
      sprintAavail.Distance = sprintData.Distance;
      sprintAavail.StartDateTime = sprintData.StartTime;
      sprintAavail.Type = sprintData.SprintType;
      sprintAavail.LocationProvided = sprintData.LocationProvided;
      sprintAavail.Lattitude = sprintData.Lattitude;
      sprintAavail.Longitutude = sprintData.Longitutude;
      sprintAavail.NumberOfParticipants = sprintData.NumberOfParticipants;
      var value = sprintAavail.Id;
      if (sprintAavail != null)
      {
        Sprint sprint = await this.SprintRepo.UpdateSprint(sprintAavail);
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
      sprintToBeCreated.NumberOfParticipants = sprintInfo.NumberOfParticipants;
      Sprint sprint = await this.SprintRepo.AddSprint(sprintToBeCreated);
      if (sprint != null)
      {
        this.SprintRepo.SaveChanges();
      }

      return sprint;
    }

  }
}