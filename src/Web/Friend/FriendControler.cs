namespace SprintCrowd.BackEnd.Web.Friend
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Friend;

    /// <summary>
    /// Handle friend related api request
    /// </summary>
    public class FriendControler : ControllerBase
    {
        /// <summary>
        /// Initialize <see cref="FriendControler"> class </see>
        /// </summary>
        /// <param name="frinedService"><see cref="IFriendService"> friend service </see></param>
        public FriendControler(IFriendService frinedService)
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
            var result = await this.FriendService.AddFriendRequest(request.UserId, request.FrinedId, request.Code);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
                Data = result,
            };
            return this.Ok(response);
        }
    }
}