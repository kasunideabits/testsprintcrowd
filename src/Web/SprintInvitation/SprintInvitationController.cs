namespace SprintCrowd.BackEnd.Web.SprintInvitation
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Domain.SprintInvitation;

    [Route("[controller]")]
    [ApiController]
    [Authorize]

    /// <summary>
    /// Sprint invitation api controller
    /// </summary>
    public class SprintInvitationController : ControllerBase
    {

        /// <summary>
        /// Initialize <see cref="SprintInvitationController"> class </see>
        /// </summary>
        /// <param name="sprintInvitationService">sprint invitation serivce</param>
        public SprintInvitationController(ISprintInvitationService sprintInvitationService)
        {
            this.SprintInvitationService = sprintInvitationService;
        }

        private ISprintInvitationService SprintInvitationService { get; }

        /// <summary>
        /// Invite friend to a sprint
        /// </summary>
        /// <param name="invite">invite request body <see cref="SprintInvitationModel"> reqeust </see></param>
        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody] SprintInvitationModel invite)
        {
            await this.SprintInvitationService.Invite(invite.InviterId, invite.InviteeId, invite.SprintId);
            return this.Ok();
        }
    }
}