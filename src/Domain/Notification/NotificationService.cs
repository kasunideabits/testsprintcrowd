namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Notification handling service
    /// </summary>
    public class NotificationService : INotificationService
    {
        /// <summary>
        /// Initialize <see cref="NotificationService"> class </see>
        /// </summary>
        /// <param name="notificationRepo">notification repo</param>
        public NotificationService(INotificationRepo notificationRepo)
        {
            this.NotificationRepo = notificationRepo;
        }

        private INotificationRepo NotificationRepo { get; }

        /// <summary>
        /// Get notificaitons related to given user id
        /// </summary>
        /// <param name="userId">user id to lookup</param>
        /// <returns><see cref="NotificationList">notificaitons realted to user </see></returns>
        public async Task<NotificationList> GetNotifications(int userId)
        {
            var notifications = await this.NotificationRepo.GetNotifications(userId);
            var result = new NotificationList();
            notifications.ForEach(n =>
            {
                switch (n.NotiticationType)
                {
                    case NotificationType.FriendInvitation:
                        break;
                    case NotificationType.SprintInvitation:
                        result.Notifications.Add(this.BuildSprintInvitation(NotificationType.SprintInvitation, n));
                        break;
                    default:
                        break;
                }
            });
            return result;
        }

        private NotificationBaseMessage BuildSprintInvitation(NotificationType notifyType, Notification notification)
        {
            NotificationBaseMessage message = new NotificationBaseMessage()
            {
                Sender = this.BuildUserInfo(notification.Sender),
                Receiver = this.BuildUserInfo(notification.Receiver),
                SendTime = notification.SendTime,
                NotificationType = notifyType,
                EventData = this.SprintInviteData(notification.SprintInvite),
            };
            return message;
        }

        private UserInfo BuildUserInfo(User user)
        {
            return new UserInfo(user.Id, user.Name, user.ProfilePicture, user.Code);
        }

        private SprintInviteInfo SprintInviteData(SprintInvite sprintInvite)
        {
            SprintInviteInfo sprintInfo = new SprintInviteInfo()
            {
                SprintInviteId = sprintInvite.Id,
                SprintId = sprintInvite.Sprint.Id,
                Name = sprintInvite.Sprint.Name,
                Distance = sprintInvite.Sprint.Distance,
                StartTime = sprintInvite.Sprint.StartDateTime,
                Status = sprintInvite.Status,
            };
            return sprintInfo;
        }
    }
}