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
        /// <summary>
        /// intializes an instance of SprintController
        /// </summary>
        /// <param name="userService">user service</param>
        /// <param name="sprintService">sprint service</param>
        public SprintController(IUserService userService, ISprintService sprintService)
        {
            this.SprintService = sprintService;
            this.UserService = userService;
        }

        private ISprintService SprintService { get; }

        private IUserService UserService { get; }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="sprint">info about the sprint</param>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateSprintModel sprint)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintService.CreateNewSprint(
                user,
                sprint.Name,
                sprint.Distance,
                sprint.StartTime,
                sprint.SprintType,
                sprint.NumberOfParticipants,
                sprint.InfluencerEmail,
                sprint.DraftEvent);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get sprint details with users who join to sprint
        /// </summary>
        /// <returns><see cref="SprintWithPariticpants">sprint details</see></returns>
        [HttpGet("{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetSprintWithPaticipants(int sprintId)
        {
            var result = await this.SprintService.GetSprintWithPaticipants(sprintId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get created sprint with userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("sprint-by-creator/{userId:int}")]
        public async Task<IActionResult> GetSprintWithPaticipantsByCreator(int userId)
        {
            var result = await this.SprintService.GetSprintByCreator(userId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }
    }
}