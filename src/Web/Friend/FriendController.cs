namespace SprintCrowd.BackEnd.Web.Friend
{
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Domain.Friend;
  using SprintCrowd.BackEnd.Domain.ScrowdUser;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Extensions;
  using System.Collections.Generic;

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
    /// <param name="frinedService"><see cref="IFriendService"> friend service </see></param>
    public FriendController(IFriendService frinedService, IUserService userService)
    {
      this.FriendService = frinedService;
      this.UserService = userService;
    }

    private IFriendService FriendService { get; }
    private IUserService UserService { get; }

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

    /// <summary>
    /// Get friends for given user
    /// </summary>
    /// <param name="userId">user id for look up</param>
    /// <returns><see cref ="FriendListDto">friend list</see></returns>
    [HttpGet("get/{userId:int}")]
    public async Task<IActionResult> GetFriends(int userId)
    {
      // var result = await this.FriendService.GetFriends(userId);
      // ResponseObject response = new ResponseObject()
      // {
      //     StatusCode = (int)ApplicationResponseCode.Success,
      //     Data = result,
      // };
      return this.Ok(userId);
    }

    /// <summary>
    /// Get friends for given user
    /// </summary>
    /// <param name="request"><see cref="FriendRequestActionModel">firend request</see></param>
    /// <returns><see cref="FriendRequestActionResult"></see> and reason</returns>
    [HttpPost("add")]
    [ProducesResponseType(typeof(ResponseObject), 200)]
    public async Task<IActionResult> PlusFriend([FromBody] FriendRequestActionModel request)
    {
      User user = await this.User.GetUser(this.UserService);
      var addedFriend = await this.FriendService.PlusFriend(user.Id, request.Code);
      var outputObj = new Dictionary<string, string>();
      outputObj.Add("name", addedFriend.Name);
      outputObj.Add("ProfilePicture", addedFriend.ProfilePicture);
      SuccessResponseObject response = new SuccessResponseObject()
      {
        data = outputObj,
      };
      return this.Ok(response);
    }
  }
}