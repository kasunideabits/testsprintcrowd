namespace SprintCrowd.BackEnd.Web.Friend
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Domain.Crons;
    using SprintCrowd.BackEnd.Domain.Friend;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Handle friend related api request
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FriendController : ControllerBase
    {
        /// <summary>
        /// Initialize <see cref="FriendController"> class </see>
        /// </summary>
        /// <param name="userService">user service</param>
        /// <param name="resetUserCodeService">resetUserCodeService service</param>
        /// <param name="frinedService"><see cref="IFriendService"> friend service </see></param>
        public FriendController(IFriendService frinedService, IUserService userService, IResetUserCodeService resetUserCodeService, ISprintParticipantService sprintParticipantService)
        {
            this.FriendService = frinedService;
            this.UserService = userService;
            this.ResetUserCodeService = resetUserCodeService;
            this.SprintParticipantService = sprintParticipantService;
        }

        private IFriendService FriendService { get; }
        private IUserService UserService { get; }
        private IResetUserCodeService ResetUserCodeService { get; }
        private ISprintParticipantService SprintParticipantService { get; }

        /// <summary>
        /// Get friends for given user
        /// </summary>
        /// <param name="request"><see cref="FriendRequestActionModel">firend request</see></param>
        [HttpPost("add")]
        [ProducesResponseType(typeof(SuccessResponse<FriendDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]

        public async Task<IActionResult> PlusFriend([FromBody] FriendRequestActionModel request)
        {
            User user = await this.User.GetUser(this.UserService);
            var addedFriend = await this.FriendService.PlusFriend(user, request.Code);
            return this.Ok(new SuccessResponse<FriendDto>(addedFriend));
        }

        /// <summary>
        /// Get all friends
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(SuccessResponse<List<FriendDto>>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> AllFriends()
        {
            User user = await this.User.GetUser(this.UserService);
            var allFriends = await this.FriendService.AllFriends(user.Id);
            return this.Ok(new SuccessResponse<List<FriendDto>>(allFriends));
        }

        /// <summary>
        /// Remove specific friend
        /// </summary>
        [HttpPost("remove")]
        [ProducesResponseType(typeof(SuccessResponse<RemoveFriendDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> RemoveFriend([FromBody] RemoveFriendActionModel request)
        {
            User user = await this.User.GetUser(this.UserService);
            var removedFriend = await this.FriendService.DeleteFriend(user.Id, request.FriendId);
            return this.Ok(new SuccessResponse<RemoveFriendDto>(removedFriend));
        }

        /// <summary>
        /// Get all friends
        /// </summary>
        [HttpGet("{userId:int}")]
        [ProducesResponseType(typeof(SuccessResponse<FriendDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> GetFriend(int userId)
        {
            var allFriends = await this.FriendService.GetFriend(userId);
            return this.Ok(new SuccessResponse<FriendDto>(allFriends));
        }


        /// <summary>
        /// Invite Send
        /// </summary>
        /// <param name="request"><see cref="FriendRequestActionModel">firend request</see></param>

        [HttpPost("InviteSend")]
        [ProducesResponseType(typeof(SuccessResponse<FriendInviteDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> InviteSend([FromBody] FriendInviteDto request)
        {
            User user = await this.User.GetUser(this.UserService);
            var addedFriend = await this.FriendService.InviteFriend(user.Id, request.ToUserId , false);
            return this.Ok(new SuccessResponse<FriendInviteDto>(addedFriend));
        }

        /// <summary>
        /// Invite Send Community
        /// </summary>
        /// <param name="request"><see cref="FriendRequestActionModel">Community firend request</see></param>

        [HttpPost("InviteSendCommunity")]
        [ProducesResponseType(typeof(SuccessResponse<FriendInviteDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> InviteSendCommunity([FromBody] FriendInviteDto request)
        {
            User user = await this.User.GetUser(this.UserService);
            var addedFriend = await this.FriendService.InviteFriend(user.Id, request.ToUserId, true);
            return this.Ok(new SuccessResponse<FriendInviteDto>(addedFriend));
        }

        [HttpGet("InviteList")]
        [ProducesResponseType(typeof(SuccessResponse<FriendInviteDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> InviteList()
        {
            User user = await this.User.GetUser(this.UserService);
            var invites = await this.FriendService.InviteList(user.Id , false);
            return this.Ok(new SuccessResponse<InviteDto>(invites));
        }

        [HttpGet("CommunityInviteList")]
        [ProducesResponseType(typeof(SuccessResponse<FriendInviteDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> CommunityInviteList()
        {
            User user = await this.User.GetUser(this.UserService);
            var invites = await this.FriendService.InviteList(user.Id , true);
            return this.Ok(new SuccessResponse<InviteDto>(invites));
        }

        /// <summary>
        /// Invite Accept
        /// </summary>
        /// <param name="inviteAccept"></param>
        /// <returns></returns>
        [HttpPost("InviteAccept")]
        [ProducesResponseType(typeof(SuccessResponse<FriendInviteDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> InviteAccept(FriendInviteAcceptDto inviteAccept)
        {
            var result = await this.FriendService.InviteAccept(inviteAccept.Id,false);
            return this.Ok(new SuccessResponse<FriendInviteDto>(result));
        }

        /// <summary>
        /// Invite Accept Community
        /// </summary>
        /// <param name="inviteAccept"></param>
        /// <returns></returns>
        [HttpPost("InviteAcceptCommunity")]
        [ProducesResponseType(typeof(SuccessResponse<FriendInviteDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> InviteAcceptCommunity(FriendInviteAcceptDto inviteAccept)
        {
            var result = await this.FriendService.InviteAccept(inviteAccept.Id, true);
            return this.Ok(new SuccessResponse<FriendInviteDto>(result));
        }

        /// <summary>
        /// Invite Delete by User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("InviteDelete/{Id}")]
        public async Task<IActionResult> InviteDelete(int Id)
        {
            var result = await this.FriendService.RemoveInvitation(Id, false);
            return this.Ok(result);
        }

        /// <summary>
        /// Community Invite Delete by User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("InviteDeleteCommunity/{Id}")]
        public async Task<IActionResult> InviteDeleteCommunity(int Id)
        {
            var result = await this.FriendService.RemoveInvitation(Id ,true);
            return this.Ok(result);
        }

        [HttpGet("GetNotificationCount/{isCommunity:bool}")]
        [ProducesResponseType(typeof(SuccessResponse<FriendInviteDto>), 200)]
        [ProducesResponseType(typeof(ErrorResponseObject), 400)]
        public async Task<IActionResult> GetNotificationCount(bool isCommunity)
        {
            User user = await this.User.GetUser(this.UserService);
            var result = this.SprintParticipantService.GetNotification(2677, isCommunity);

            int count = result != null?result.ResultNew.Count + result.ResultOlder.Count + result.ResultToday.Count : 0;
            return this.Ok(new SuccessResponse<int>(count));
        }



    }
}