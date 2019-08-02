namespace SprintCrowd.BackEnd.Web.Sprint
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Web.Event;

    /// <summary>
    /// Controller for handle sprint participants
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    // [Authorize]
    public class SprintParticipantController : ControllerBase
    {
        /// <summary>
        /// Initialize SprintMarkAttendanceController controller
        /// </summary>
        /// <param name="userService">instance reference for IUserService</param>
        /// <param name="sprintParticipantService">instance reference for ISprintParticipantService</param>
        public SprintParticipantController(
            IUserService userService,
            ISprintParticipantService sprintParticipantService)
        {
            this.UserService = userService;
            this.SprintParticipantService = sprintParticipantService;
        }

        private IUserService UserService { get; }
        private ISprintParticipantService SprintParticipantService { get; }

        /// <summary>
        /// Mark attenedance for given sprint id
        /// </summary>
        /// <param name="markAttendence">sprint and user details</param>
        [HttpPost("mark-attendence")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> MarkAttendence([FromBody] MarkAttendence markAttendence)
        {
            await this.SprintParticipantService.MarkAttendence(markAttendence.SprintId, markAttendence.UserId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = "Successfully update mark attendence",
            };
            return this.Ok(response);
        }

        /// <summary>
        /// creates an event
        /// </summary>
        /// <param name="joinUser"><see cref="JoinPrivateSprintModel"> join user data </see></param>
        // TODO handle bad request
        [HttpPost("join")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        [ProducesResponseType(typeof(ResponseObject), 400)]
        public async Task<IActionResult> JoinEvent([FromBody] JoinPrivateSprintModel joinUser)
        {
            // User user = await this.User.GetUser(this.UserService);
            // if (user.Id != joinUser.UserId)
            // {
            //     return this.BadRequest();
            // }
            await this.SprintParticipantService.JoinSprint(joinUser.SprintId, joinUser.UserId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = "Successfully joined for a sprint",
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Exit an event
        /// </summary>
        /// <param name="exitEvent">Exit event informantion</param>
        [HttpPost("exit")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> ExitEvent([FromBody] ExitEventModel exitEvent)
        {
            ExitSprintResult result = await this.SprintParticipantService.ExitSprint(exitEvent.SprintId, exitEvent.UserId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get all participant who join with given sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to look up</param>
        /// <returns>list of <see cref="ParticipantInfo">participants</see></returns>
        [HttpGet("join/{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetJoinParticipants(int sprintId)
        {
            var result = await this.SprintParticipantService.GetParticipants(sprintId, ParticipantStage.JOINED);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

    }
}