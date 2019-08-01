namespace SprintCrowd.BackEnd.Web.Friend
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Friend;

    /// <summary>
    /// Handle friend related api request
    /// </summary>
    public class FriendController : ControllerBase
    {
        /// <summary>
        /// Initialize <see cref="FriendController"> class </see>
        /// </summary>
        /// <param name="frinedService"><see cref="IFriendService"> friend service </see></param>
        public FriendController(IFriendService frinedService)
        {
            this.FriendService = frinedService;
        }

        private IFriendService FriendService { get; }

        /// <summary>
        /// Add friend request
        /// </summary>
        /// <returns><see cref="">sprint details</see></returns>
        [HttpPost("add-request")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> AddFriend([FromBody] AddFriendModel request)
        {
            var result = await this.FriendService.AddFriendRequest(request.UserId, request.FriendId, request.Code);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get friend details with given friend id
        /// </summary>
        /// <param name="friendId">friend id</param>
        /// <returns><see cref="FriendDto"> friend details </returns>
        [HttpGet("get/{fiendId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetFriend(int friendId)
        {
            var result = await this.FriendService.GetFriend(friendId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Get all friends for given user id
        /// </summary>
        /// <param name="userId">user id for get friend list</param>
        /// <returns><see cref="FriendListDto">friend list</returns>
        [HttpGet("get-all/{userId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetFriends(int userId)
        {
            var result = await this.FriendService.GetFriends(userId);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }

        /// <summary>
        /// Remove friend from friend list
        /// </summary>
        /// <param name="remove"><see cref="RemoveFriendModel"> request body</see></param>
        /// <returns>empty body</returns>
        [HttpGet("remove")]
        public async Task<IActionResult> RemoveFriend([FromBody] RemoveFriendModel remove)
        {
            await this.FriendService.RemoveFriend(remove.UserId, remove.FriendId);
            return this.Ok();
        }
    }
}