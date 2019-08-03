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