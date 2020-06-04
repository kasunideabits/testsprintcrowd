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
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Handle friend related api request
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class FriendController : ControllerBase
    {
        /// <summary>
        /// Initialize <see cref="FriendController"> class </see>
        /// </summary>
        /// <param name="userService">user service</param>
        /// <param name="resetUserCodeService">resetUserCodeService service</param>
        /// <param name="frinedService"><see cref="IFriendService"> friend service </see></param>
        public FriendController(IFriendService frinedService, IUserService userService, IResetUserCodeService resetUserCodeService)
        {
            this.FriendService = frinedService;
            this.UserService = userService;
            this.ResetUserCodeService = resetUserCodeService;
        }

        private IFriendService FriendService { get; }
        private IUserService UserService { get; }
        private IResetUserCodeService ResetUserCodeService { get; }

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
            var addedFriend = await this.FriendService.PlusFriend(user.Id, request.Code);
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
    }
}