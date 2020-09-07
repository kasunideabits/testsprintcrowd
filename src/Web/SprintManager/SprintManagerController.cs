namespace SprintCrowd.BackEnd.SprintManager.Web
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Domain.Achievement;
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
        /// <param name="achievementService">sprint paritcipant service</param>
        public SprintManagerController(ISprintParticipantService participantService, IAchievementService achievementService)
        {
            this.SprintParticipantService = participantService;
            this.AchievementService = achievementService;
        }
        private ISprintParticipantService SprintParticipantService { get; }
        private IAchievementService AchievementService { get; }

        /// <summary>
        /// Update participant completed the sprint
        /// </summary>
        /// <param name="race">event details</param>
        [HttpPost("sprint/completed")]
        public async Task<IActionResult> RaceCompleted([FromBody] EventStatusModel race)
        {
            await this.SprintParticipantService.UpdateParticipantStatus(race.UserId, race.SprintId, race.Time, ParticipantStage.COMPLETED);
            var sprint = await this.SprintParticipantService.GetSprint(race.SprintId);
            var achievements = await this.AchievementService.RaceCompleted(race.UserId, (SprintType)sprint.SprintType);
            return this.Ok(new SuccessResponse<List<SprintCrowd.Domain.Achievement.AchievementDto>>(achievements));
        }

        /// <summary>
        /// Update participant exit the sprint
        /// </summary>
        /// <param name="race">event details</param>
        [HttpPost("sprint/exit")]
        public async Task<IActionResult> RaceExited([FromBody] EventStatusModel race)
        {
            await this.SprintParticipantService.UpdateParticipantStatus(race.UserId, race.SprintId, race.Time, ParticipantStage.QUIT);
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