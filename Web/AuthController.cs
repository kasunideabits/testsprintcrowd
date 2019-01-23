namespace SprintCrowd.Backend.Web
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// User authentication controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        /// <summary>
        /// Performs a login using a token provided by an external login provider.
        /// </summary>
        /// <param name="token">The <see cref="ExternalLoginToken"/>.</param>
        /// <returns>The result of the login operation.</returns>
        /// <response code="200">The result of the login operation.</response>
        /// <response code="400">The token is invalid.</response>
        [HttpPost("external-login")]
        [ProducesResponseType( typeof(ExternalLoginResult), 200)]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginToken token)
        {
            return this.Ok(new ExternalLoginResult());
        }
    }
}