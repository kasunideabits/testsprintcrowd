namespace SprintCrowd.BackEnd.SprintManager.Web
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Web.SprintManager;

    /// <summary>
    /// Endpoint for handle sprint manager request for update participants and achivements
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SprintManagerController : ControllerBase
    {
        /// <summary>
        /// Initialize SprintManagerController class
        /// </summary>
        /// <param name="participantService">sprint paritcipant service</param>
        public SprintManagerController(ISprintParticipantService participantService)
        {
            this.SprintParticipantService = participantService;
        }
        private ISprintParticipantService SprintParticipantService { get; }

        /// <summary>
        /// Update participant completed the sprint
        /// </summary>
        /// <param name="race">event details</param>
        [HttpPost("sprint/completed")]
        public async Task<IActionResult> RaceCompleted([FromBody] EventStatusModel race)
        {
            await this.SprintParticipantService.UpdateParticipantStatus(race.UserId, race.SprintId, race.Distance, race.Time, ParticipantStage.COMPLETED);
            return this.Ok();
        }

        /// <summary>
        /// Update participant exit the sprint
        /// </summary>
        /// <param name="race">event details</param>
        [HttpPost("sprint/exit")]
        public async Task<IActionResult> RaceExited([FromBody] EventStatusModel race)
        {
            await this.SprintParticipantService.UpdateParticipantStatus(race.UserId, race.SprintId, race.Distance, race.Time, ParticipantStage.QUIT);
            return this.Ok();
        }

        /// <summary>
        /// Sprint expired handler
        /// </summary>
        [HttpPost("sprint/expired")]
        public async Task<IActionResult> SprintExpired([FromBody] NotCompletedRunnerModel notCompletedRunner)
        {
            await this.SprintParticipantService.SprintExpired(notCompletedRunner.SprintId, notCompletedRunner.Runners);
            return this.Ok();
        }
    }
}