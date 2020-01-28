namespace SprintCrowd.BackEnd.Web.PushNotification
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// push notification controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PushNotificationController : ControllerBase
    {
        private readonly IUserService userService;

        /// <summary>
        /// initializes a instance of PushNotificationController class.
        /// </summary>
        /// <param name="userService">user service</param>
        public PushNotificationController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// gets the fcm token from mobile
        /// </summary>
        /// <param name="fcmModel">fcm token</param>
        /// <returns>a response object containing the request response code</returns>
        [HttpPost("savefcmtoken")]
        [ProducesResponseType(typeof(ResponseObject), 200)]
        public async Task<IActionResult> SaveFcmToken([FromBody] FcmModel fcmModel)
        {
            System.Console.WriteLine("=================================================== Save Push Notification");
            User user = await this.User.GetUser(this.userService);
            System.Console.WriteLine($"=================================================== Save Push Notification { user} , {user.Name}");

            await this.userService.SaveFcmToken(user.Id, fcmModel.Token);
            ResponseObject response = new ResponseObject()
            {
                StatusCode = (int)ApplicationResponseCode.Success,
            };
            return this.Ok(response);
        }

    }
}