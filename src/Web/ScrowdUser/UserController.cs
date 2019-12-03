namespace SprintCrowd.Web.ScrowdUser
{
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using SprintCrowd.BackEnd.Application;
  using SprintCrowd.BackEnd.Common;
  using SprintCrowd.BackEnd.Domain.ScrowdUser.Dtos;
  using SprintCrowd.BackEnd.Domain.ScrowdUser;
  using SprintCrowd.BackEnd.Extensions;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
  using SprintCrowd.BackEnd.Infrastructure.Persistence;
  using SprintCrowd.BackEnd.Web.ScrowdUser;

  /// <summary>
  /// User controller.
  /// </summary>
  [Route("[controller]")]
  [ApiController]
  [Authorize]
  public class UserController : ControllerBase
  {
    /// <summary>
    /// Initialize <see cref="UserController"> class </see>
    /// </summary>
    /// <param name="userService">user service instance</param>
    public UserController(IUserService userService)
    {
      this.UserService = userService;
    }

    private IUserService UserService { get; set; }
    private ScrowdDbContext Context { get; set; }

    /// <summary>
    /// Get authorized user details
    /// </summary>
    [HttpGet("get")]
    public async Task<IActionResult> GetUser()
    {
      var authorizedUser = await this.User.GetUser(this.UserService);
      var userResult = await this.UserService.GetUser(authorizedUser.Id);
      ResponseObject response = new ResponseObject()
      {
        StatusCode = (int)ApplicationResponseCode.Success,
        Data = userResult,
      };
      return this.Ok(response);
    }

    /// <summary>
    /// Update user activity
    /// </summary>
    [HttpGet("updateActivity")]
    public async Task<IActionResult> UpdateUserActivity()
    {
      var authorizedUser = await this.User.GetUser(this.UserService);
      UserActivity activity = await this.UserService.UpdateUserActivity(authorizedUser.Id);
      ResponseObject response = new ResponseObject()
      {
        StatusCode = (int)ApplicationResponseCode.Success,
        Data = activity,
      };
      return this.Ok(response);
    }

    /// <summary>
    /// Get user pereference
    /// </summary>
    /// <returns>user peference</returns>
    [HttpGet("preference")]
    [ProducesResponseType(typeof(SuccessResponse<UserPreferenceDto>), 200)]
    [ProducesResponseType(typeof(SuccessResponse<ErrorResponseObject>), 400)]
    public async Task<IActionResult> UserPreference()
    {
      var authorizedUser = await this.User.GetUser(this.UserService);
      var result = await this.UserService.GetUserPreference(authorizedUser.Id);
      return this.Ok(new SuccessResponse<UserPreferenceDto>(result));
    }
  }
}