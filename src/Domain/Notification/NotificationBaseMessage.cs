namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class NotificationBaseMessage
    {
        public UserInfo Sender { get; set; }

        public UserInfo Receiver { get; set; }

        public NotificationType NotificationType { get; set; }

        public DateTime SendTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public dynamic EventData { get; set; }
    }
}