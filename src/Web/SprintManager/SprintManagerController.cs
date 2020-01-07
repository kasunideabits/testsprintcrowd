namespace SprintCrowd.BackEnd.SprintManager.Web
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;

    /// <summary>
    /// Endpoint for handle sprint manager request for update participants and achivements
    /// </summary>
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
        [HttpGet("sprint/completed")]
        public async Task<IActionResult> RaceCompleted(EventStatusModel race)
        {
            await this.SprintParticipantService.UpdateParticipantStatus(race.UserId, race.SprintId, race.Distance, race.Time, ParticipantStage.COMPLETED);
            return this.Ok();
        }

        /// <summary>
        /// Update participant exit the sprint
        /// </summary>
        /// <param name="race">event details</param>
        [HttpGet("sprint/exit")]
        public async Task<IActionResult> RaceExited(EventStatusModel race)
        {
            await this.SprintParticipantService.UpdateParticipantStatus(race.UserId, race.SprintId, race.Distance, race.Time, ParticipantStage.QUIT);
            return this.Ok();
        }

        /// <summary>
        /// Sprint expired handler
        /// </summary>
        /// <param name="sprintId"></param>
        [HttpGet("sprint/1")]
        public async Task<IActionResult> SprintExpired(int sprintId)
        {
            System.Console.WriteLine(sprintId);
            // this.SprintParticipantService.UpdateParticipantStatus(race.UserId, race.SprintId, race.ComplteTime, ParticipantStage.QUIT);
            return this.Ok();
        }
    }
}