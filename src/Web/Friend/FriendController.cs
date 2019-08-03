namespace SprintCrowd.BackEnd.Web.Friend
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Friend;

    /// <summary>
    /// Handle friend related api request
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    // [Authorize]
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
        /// Generate friend request code
        /// </summary>
        /// <returns><see cref="GenerateFriendCodeModel">sprint details</see></returns>
        [HttpPost("generate-code")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GenerateFriendCode([FromBody] GenerateFriendCodeModel request)
        {
            var result = await this.FriendService.GenerateFriendCode(request.UserId, request.Code);
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
        /// <param name="query">Query string for get friends</param>
        /// <param name="userId">userId id</param>
        /// <returns><see cref="FriendDto"> friend details </see></returns>
        [HttpGet("get/{userId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetFriend([FromQuery] GetFriendQuery query, int userId)
        {
            var result = await this.FriendService.GetFriend(userId, query.FriendId, query.RequestStatus);
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
        /// <param name="query">query string parameters</param>
        /// <returns><see cref="FriendListDto">friend list </see> </returns>
        [HttpGet("get-all/{userId:int}")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> GetFriends([FromQuery] GetAllFriendQuery query, int userId)
        {
            if (query.RequestStatus == null)
            {
                var result = await this.FriendService.GetAllFriends(userId);
                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.Success,
                    Data = result,
                };
                return this.Ok(response);
            }
            else
            {
                var result = await this.FriendService.GetFriends(userId, query.RequestStatus);
                ResponseObject response = new ResponseObject()
                {
                    StatusCode = (int)ApplicationResponseCode.Success,
                    Data = result,
                };
                return this.Ok(response);
            }
        }

        /// <summary>
        /// Remove friend from friend list
        /// </summary>
        /// <param name="remove"><see cref="RemoveFriendModel"> request body</see></param>
        /// <returns>empty body</returns>
        [HttpDelete("remove")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        [ProducesResponseType(typeof(RemoveFriendResult), 400)]
        public async Task<IActionResult> RemoveFriend([FromBody] RemoveFriendModel remove)
        {
            if (remove.UserId == 0)
            {
                return this.BadRequest(RemoveFriendResult.NullUserId());
            }
            if (remove.FriendId == 0)
            {
                return this.BadRequest(RemoveFriendResult.NullFriendId());
            }
            await this.FriendService.RemoveFriend(remove.UserId, remove.FriendId);
            return this.Ok();
        }
    }
}