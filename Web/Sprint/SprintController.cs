namespace SprintCrowd.BackEnd.Web.Event
{
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Domain.ScrowdUser;
  using SprintCrowd.BackEnd.Domain.Sprint;
  using SprintCrowd.BackEnd.Extensions;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  /// event controller
  /// </summary>
  [Route("[controller]")]
  [ApiController]
  [Authorize]

  public class SprintController : ControllerBase
  {
    private ISprintService SprintService;
    private IUserService UserService;
    /// <summary>
    /// intializes an instance of SprintController
    /// </summary>
    /// <param name="sprintService">sprint service</param>
    /// /// <param name="userService">user service</param>
    public SprintController(ISprintService sprintService, IUserService userService)
    {
      this.SprintService = sprintService;
      this.UserService = userService;
    }

    /// <summary>
    /// creates an event
    /// </summary>
    /// <param name="sprintInfo">info about the sprint</param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    public async Task<ResponseObject> CreateEvent([FromBody] SprintModel sprintInfo)
    {
      User user = await this.User.GetUser(this.UserService);
      var result = this.SprintService.CreateNewSprint(sprintInfo, user);

      return new ResponseObject()
      {
        StatusCode = (int)ApplicationResponseCode.Success,
        Data = result
      };
    }
  }
}