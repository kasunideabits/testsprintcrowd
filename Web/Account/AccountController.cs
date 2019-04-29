namespace SprintCrowdBackEnd.Web.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.Backend.Application;
    using SprintCrowdBackEnd.Application;
    using SprintCrowdBackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// account controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;
        /// <summary>
        /// No authorization is happening here, just registering a new user.
        /// Sends request to authorization server, authorization server registers and returns user data, those data are saved in the
        /// app database.
        /// Authorization happens in identity server
        /// </summary>
        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Register user
        /// TODO: switch user type
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<ResponseObject> Register([FromBody] RegisterModel registerData)
        {
            User user = await this.userService.RegisterUser(registerData);
            return new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = user };
        }
    }
}