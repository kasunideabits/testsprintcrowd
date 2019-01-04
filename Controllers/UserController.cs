using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SprintCrowdBackEnd.interfaces;
using SprintCrowdBackEnd.Models;

namespace SprintCrowdBackEnd.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(new ResponseObject(){
                Data = user
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]User userParam)
        {
            ResponseObject response = _userService.RegisterUser(userParam);

            return Ok(response);
        }

        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok(new ResponseObject() {
                Data = User.Identity.Name
            });
        }

    }
}