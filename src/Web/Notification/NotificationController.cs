namespace SprintCrowd.BackEnd.Web.Notification
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Domain.Notification;

    /// <summary>
    /// Notificaiton handling controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    //[Authorize]

    public class NotificationController : ControllerBase
    {
        /// <summary>
        /// Initialize  <see cref="NotificationController"> class </see>
        /// </summary>
        /// <param name="notificationService">notification service instance</param>
        public NotificationController(INotificationService notificationService)
        {
            this.NotificationService = notificationService;
        }

        private INotificationService NotificationService { get; }

        /// <summary>
        /// Get notificaitons related to given user id
        /// </summary>
        /// <param name="userId">user id to lookup</param>
        /// <returns></returns>
        [HttpGet("get/{userId:int}")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            var result = await this.NotificationService.GetNotifications(userId);
            return this.Ok(result);
        }
    }
}