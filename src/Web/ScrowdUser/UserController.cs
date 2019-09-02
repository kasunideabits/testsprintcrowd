namespace SprintCrowd.Web.ScrowdUser
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Extensions;

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
    }
}