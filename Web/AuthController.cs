namespace SprintCrowd.Backend.Web
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.Backend.Infrastructure.ExternalLogin;
    using SprintCrowd.Backend.Application;

    /// <summary>
    /// User authentication controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IFacebookAuthService facebookAuthenticationService;

        /// <summary>
        /// Initialices a new instance of <see cref="AuthController" />.
        /// </summary>
        /// <param name="facebookAuthenticationService">The facebook authentication service.</param>
        public AuthController(IFacebookAuthService facebookAuthenticationService)
        {
            this.facebookAuthenticationService = facebookAuthenticationService;
        }
        

        /// <summary>
        /// Authenticates/registers a user via facebook.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("facebook")]
        [ProducesResponseType( typeof(ExternalLoginResult), 200)]
        [ProducesResponseType( typeof(ErrorResponse), 400)]
        [ProducesResponseType( typeof(ErrorResponse), 500)]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookAuthInfo token)
        {
            ExternalLoginResult result = await this.facebookAuthenticationService.Authencticate(token);

            return Ok(result);
        }
    }
}