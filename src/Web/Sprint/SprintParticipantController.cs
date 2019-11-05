namespace SprintCrowd.BackEnd.Web.Sprint
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Event;
    using SprintCrowd.BackEnd.Web.Sprint.Models;

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
        /// <param name="markAttendance">sprint and user details</param>
        [HttpPost("mark-attendance")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> MarkAttendence([FromBody] MarkAttendance markAttendance)
        {
            await this.SprintParticipantService.MarkAttendence(markAttendance.SprintId, markAttendance.UserId);
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
        /// <param name="joinUser"><see cref="JoinSprintModel"> join user data </see></param>
        [HttpPost("join")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> JoinEvent([FromBody] JoinSprintModel joinUser)
        {
            User user = await this.User.GetUser(this.UserService);
            await this.SprintParticipantService.JoinSprint(
                joinUser.SprintId,
                user.Id,
                joinUser.Status);
            await this.SprintParticipantService.RemoveNotification(joinUser.NotificationId);
            return this.Ok();
        }

        /// <summary>
        /// Get all sprints with given user id
        /// </summary>
        /// <param name="query">query params for filter sprints</param>
        /// <param name="userId">user id to look up</param>
        /// <returns><see cref="SprintInfo"> all sprints </see></returns>
        [HttpGet("all/{userId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        [ProducesResponseType(typeof(ResponseObject), 400)]
        public IActionResult GetSprints([FromQuery] SprintQuery query, int userId)
        {
            var result = this.SprintParticipantService.GetSprints(
                userId,
                query.SprintType,
                query.ParticipantStage,
                query.DistanceFrom,
                query.DistanceTo,
                query.StartFrom,
                query.CurrentTimeBuff);

            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
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

        /// <summary>
        /// Get all participant who join with given sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to look up</param>
        /// <returns>list of <see cref="ParticipantInfo">participants</see></returns>
        [HttpGet("mark-attendance/{sprintId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetMarkAttendanceParticipants(int sprintId)
        {
            var result = await this.SprintParticipantService.GetParticipants(sprintId, ParticipantStage.MARKED_ATTENDENCE);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        [HttpGet("marked-attendance-sprint/{userId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetMarkAttendanceSprint(int userId)
        {
            var result = await this.SprintParticipantService.GetSprintWhichMarkedAttendance(userId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Invite friend to a sprint
        /// </summary>
        /// <param name="invite">invite request body <see cref="SprintInvitationModel"> reqeust </see></param>
        [HttpPost("invite-request")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Invite([FromBody] SprintInvitationModel invite)
        {
            await this.SprintParticipantService.SprintInvite(invite.SprintId, invite.InviterId, invite.InviteeIds);
            return this.Ok();
        }

        [HttpGet("notification")]
        [ProducesResponseType(typeof(SuccessResponse<>), 200)]
        public async Task<IActionResult> GetNotification()
        {
            User user = await this.User.GetUser(this.UserService);
            var result = await this.SprintParticipantService.GetNotification(user.Id);
            return this.Ok(new SuccessResponse<List<dynamic>>(result));
        }

        [HttpDelete("participant/{sprintId:int}/{participantId:int}/")]
        public async Task<IActionResult> RemoveParticipant(int sprintId, int participantId)
        {
            User user = await this.User.GetUser(this.UserService);
            await this.SprintParticipantService.RemoveParticipant(user.Id, sprintId, participantId);
            return this.Ok();
        }

        [HttpGet("friends/{sprintId:int}")]
        public async Task<IActionResult> GetFriendsStatusInSprint(int sprintId)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = this.SprintParticipantService.GetFriendsStatusInSprint(user.Id, sprintId);
            return this.Ok(new SuccessResponse<List<FriendInSprintDto>>(result));
        }
    }
}