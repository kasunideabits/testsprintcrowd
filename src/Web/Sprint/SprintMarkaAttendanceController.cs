namespace SprintCrowd.BackEnd.Web.Sprint
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint;

    public class SprintMarkaAttendanceController : ControllerBase
    {
        public SprintMarkaAttendanceController(ISprintService sprintService)
        {
            this.SprintService = sprintService;
        }

        private ISprintService SprintService { get; }

        [HttpGet("mark-attendence")]
        //   [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> MarkAttendence([FromBody] MarkAttendence markAttendence)
        {
            System.Console.WriteLine(markAttendence);
            return this.Ok();
        }
    }
}