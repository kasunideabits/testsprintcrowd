namespace SprintCrowd.BackEnd.Web.Sprint
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;

    /// <summary>
    /// Controller for handle sprint participants
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SprintMarkAttendanceController : ControllerBase
    {
        /// <summary>
        /// Initialize SprintMarkAttendanceController controller
        /// </summary>
        /// <param name="sprintParticipantService">instance reference for ISprintParticipantService</param>
        public SprintMarkAttendanceController(ISprintParticipantService sprintParticipantService)
        {
            this.SprintParticipantService = sprintParticipantService;
        }

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
    }
}