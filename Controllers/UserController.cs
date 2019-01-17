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

        /*
            Will both register and login.
            validate the access token, if account exists send jwt token, if not register
            and then send jwt token

            Input: FbAccessToken => String
         */
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var fbAccessToken = userParam.Token;
            
            var user = _userService.Authenticate(fbAccessToken);

            if (user == null)
            {
                return BadRequest(new ResponseObject(){
                    StatusId = (int)Enums.ResponseStatus.IncorrectAuth,
                    Data = "Error occured while logging you in, Please try again later"
                });
            }
        
            return Ok(new ResponseObject(){
                Data = user
            });
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