namespace SprintCrowd.BackEnd.Web.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Achievement;
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
        private readonly IAchievementService AchievementService;

        /// <summary>
        /// No authorization is happening here, just registering a new user.
        /// Sends request to authorization server, authorization server registers and returns user data, those data are saved in the
        /// app database.
        /// Authorization happens in identity server
        /// </summary>
        public AccountController(IUserService userService, IAchievementService achievementService)
        {
            this.UserService = userService;
            this.AchievementService = achievementService;
        }

        /// <summary>
        /// Register user
        /// TODO: switch user type
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerData)
        {


            var email = registerData.Email;
            var encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(email);

            registerData.Email = encryptedEamil;
            //var decryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(encryptedEamil);


            User user = await this.UserService.RegisterUser(registerData);
            await this.AchievementService.SignUp(user.Id);
            return this.Ok(new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = user });
        }
        /// TODO: switch user type
        /// </summary>
        [HttpPost("registerByMail")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> RegisterByMail([FromBody] EmailUser registerData)
        {
            var email = registerData.Email;
            var encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(email);

            registerData.Email = encryptedEamil;
            //var decryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.DecryptString(encryptedEamil);

            User user = await this.UserService.RegisterEmailUser(registerData);
            await this.AchievementService.SignUp(user.Id);
            return this.Ok(new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = user });
        }

        /// <summary>
        /// Save Promotion Code
        /// </summary>
        /// <param name="registerDatal"></param>
        /// <returns></returns>
        [HttpPost("SavePromotionCode")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> SavePromotionCode([FromBody] EmailUser registerDatal)
        {
            await this.UserService.AddPromotionCode(registerDatal);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
            };
            return this.Ok(response);
        }

        /// <summary>
        ///  Email Confirmation By Mail
        /// </summary>
        /// <param name="registerData"></param>
        /// <returns></returns>
        [HttpPost("EmailConfirmationByMail")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> EmailConfirmationByMail([FromBody] EmailUser registerData)
        {

            var email = registerData.Email;
            //var encryptedEamil = Common.EncryptionDecryptionUsingSymmetricKey.EncryptString(email);

            //registerData.Email = encryptedEamil;
            bool success = await this.UserService.EmailConfirmationByMail(registerData);
            //await this.AchievementService.SignUp(user.Id);
            return this.Ok(new ResponseObject { StatusCode = (int)ApplicationResponseCode.Success, Data = success });
        }
    }
}