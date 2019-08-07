namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Base message for notifiations
    /// </summary>
    public class NotificationBaseMessage
    {
        /// <summary>
        /// Who send the notificaiton
        /// </summary>
        public UserInfo Sender { get; set; }

        /// <summary>
        /// Who receive the notificaiton
        /// </summary>
        public UserInfo Receiver { get; set; }

        /// <summary>
        /// Type of the notification
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// Send time
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Read time
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Event realted data based on notificaiton type
        /// </summary>
        public dynamic EventData { get; set; }
    }
}