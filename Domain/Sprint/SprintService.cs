namespace SprintCrowdBackEnd.Domain.Sprint
{
  /// <summary>
  /// Sprint service
  /// </summary>
  public class SprintService
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
  }
}