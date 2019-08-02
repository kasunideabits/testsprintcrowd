namespace SprintCrowd.BackEnd.Web.Sprint
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Event;

    /// <summary>
    /// Controller for handle sprint participants
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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
        /// <param name="modelInfo">Id of the sprint</param>
        [HttpPost("join")]
        public async Task<ResponseObject> JoinEvent([FromBody] JoinPrivateSprintModel modelInfo)
        {
            User user = await this.User.GetUser(this.UserService);
            if (modelInfo.IsConfirmed)
            {
                var result = await this.SprintParticipantService.CreateSprintJoinee(modelInfo, user);

                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.Success,
                    Data = result,
                };
                return response;
            }
            else
            {
                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.BadRequest,
                };
                return response;
            }
        }

        /// <summary>
        /// Exit an event
        /// </summary>
        /// <param name="exitEvent">Exit event informantion</param>
        [HttpPost("exit")]
        public async Task<ResponseObject> ExitEvent([FromBody] ExitEventModel exitEvent)
        {
            ExitSprintResult result = await this.SprintParticipantService.ExitSprint(exitEvent.SprintId, exitEvent.UserId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return response;
        }
    }
}