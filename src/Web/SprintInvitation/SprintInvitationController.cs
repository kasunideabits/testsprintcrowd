namespace SprintCrowd.BackEnd.Web.SprintInvitation
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Domain.SprintInvitation;

    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class SprintInvitationController : ControllerBase
    {

        public SprintInvitationController(ISprintInvitationService sprintInvitationService)
        {
            this.SprintInvitationService = sprintInvitationService;
        }

        private ISprintInvitationService SprintInvitationService { get; }

        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody] SprintInvitationModel invite)
        {
            await this.SprintInvitationService.Invite(invite.InviterId, invite.InviteeId, invite.SprintId);
            return this.Ok();
        }
    }
}