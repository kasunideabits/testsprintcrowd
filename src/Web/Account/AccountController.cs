namespace SprintCrowd.BackEnd.Web.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// account controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService UserService;
        /// <summary>
        /// No authorization is happening here, just registering a new user.
        /// Sends request to authorization server, authorization server registers and returns user data, those data are saved in the
        /// app database.
        /// Authorization happens in identity server
        /// </summary>
        public AccountController(IUserService userService)
        {
            this.UserService = userService;
        }

        /// <summary>
        /// Register user
        /// TODO: switch user type
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerData)
        {
            User user = await this.UserService.RegisterUser(registerData);
            return this.Ok(new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = user });
        }
    }
}