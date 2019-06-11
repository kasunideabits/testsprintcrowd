namespace SprintCrowdBackEnd.Web.Event
{
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc;
  using SprintCrowd.Backend.Application;
  using SprintCrowdBackEnd.Application;
  using SprintCrowdBackEnd.Domain.Sprint;
  using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
  /// <summary>
  /// event controller
  /// </summary>

  [Route("[controller]")]
  [ApiController]
  public class EventController : ControllerBase
  {

    private readonly ISprintService sprintService;

    public EventController(ISprintService sprintService)
    {
      this.sprintService = sprintService;
    }

    [HttpPost]
    [Route("create")]
    public ResponseObject CreateEvent() => null;

    /// <summary>
    /// update sprint
    /// </summary>
    [HttpPut]
    [Route("update")]
    public async Task<ResponseObject> UpdateEvent([FromBody] SprintModel SprintData)
    {
      Sprint sprint = await this.sprintService.UpdateSprint(SprintData);

      return new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = sprint };
    }

  }
}