namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class NotificationService : INotificationService
    {
        public NotificationService(INotificationRepo notificationRepo)
        {
            this.NotificationRepo = notificationRepo;
        }

        private INotificationRepo NotificationRepo { get; }
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
                        result.Notifications.Add(this.BuildSprintInvitation(n));
                        break;
                    default:
                        break;
                }
            });
            return result;
        }

        private NotificationBaseMessage BuildSprintInvitation(Notification notification)
        {
            NotificationBaseMessage message = new NotificationBaseMessage()
            {
                Sender = this.BuildUserInfo(notification.Sender),
                Receiver = this.BuildUserInfo(notification.Receiver),
                SendTime = notification.SendTime,
                EventData = this.SprintDate(notification.Sprint),
            };
            return message;
        }

        private UserInfo BuildUserInfo(User user)
        {
            return new UserInfo(user.Id, user.Name, user.ProfilePicture, user.Code);
        }

        private SprintInfo SprintDate(Sprint sprint)
        {
            SprintInfo sprintInfo = new SprintInfo()
            {
                SprintId = sprint.Id,
                Name = sprint.Name,
                Distance = sprint.Distance,
                StartTime = sprint.StartDateTime,
            };
            return sprintInfo;
        }
    }
}