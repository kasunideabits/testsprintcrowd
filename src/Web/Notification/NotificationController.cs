namespace SprintCrowd.BackEnd.Web.Notification
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SprintCrowd.BackEnd.Domain.Notification;

    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class NotificationController : ControllerBase
    {
        public NotificationController(INotificationService notificationService)
        {
            this.NotificationService = notificationService;
        }

        private INotificationService NotificationService { get; }

        [HttpGet("get/{userId:int}")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            var result = await this.NotificationService.GetNotifications(userId);
            return this.Ok(result);
        }
    }
}