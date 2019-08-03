namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System;

    public class NotificationBaseMessage
    {
        public UserInfo Sender { get; set; }

        public UserInfo Receiver { get; set; }

        public DateTime SendTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public dynamic EventData { get; set; }
    }
}