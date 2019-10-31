namespace SprintCrowd.BackEnd.Web.SprintInvitation
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Domain.SprintInvitation;

    /// <summary>
    /// Sprint invitation api controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SprintInvitationController : ControllerBase
    {

        /// <summary>
        /// Initialize <see cref="SprintInvitationController"> class </see>
        /// </summary>
        /// <param name="sprintService">sprint serivce</param>
        public SprintInvitationController(ISprintService sprintService)
        {
            this.SprintService = sprintService;
        }

        private ISprintService SprintService { get; }

        /// <summary>
        /// Invite friend to a sprint
        /// </summary>
        /// <param name="invite">invite request body <see cref="SprintInvitationModel"> reqeust </see></param>
        [HttpPost("invite-request")]
        public async Task<IActionResult> Invite([FromBody] SprintInvitationModel invite)
        {
            try
            {
                await this.SprintService.InviteRequest(invite.InviterId, invite.InviteeId, invite.SprintId);
                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.Success,
                };
                return this.Ok(response);
            }
            catch (ApplicationException ex)
            {

                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.BadRequest,
                    Data = new { ErrorCode = ex.ErrorCode, Reason = ex.Message.ToString() }
                };
                return this.Ok(response);
            }
        }
    }
}