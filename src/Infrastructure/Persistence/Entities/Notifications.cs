namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;

    /// <summary>
    /// Class define Notification table attributes
    /// </summary>
    public class Notifications
    {
        /// <summary>
        /// Unique id for notificaiton
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Type of notification
        /// </summary>
        /// <value></value>
        public NotificationType NotiticationType { get; set; }

        /// <summary>
        /// Id reference for notificaiton sender
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Id for notification receiver
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// Send time of the notification
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Notificaiton read or not
        /// </summary>
        /// <value></value>
        public bool IsRead { get; set; }

        /// <summary>
        /// Reference for notificaiton sender
        /// </summary>
        public virtual User Sender { get; set; }

        /// <summary>
        /// Reference for notification receiver
        /// </summary>
        public virtual User Receiver { get; set; }

        /// <summary>
        /// Reference for sprint invitiation notification
        /// </summary>
        public virtual SprintInvitationNotification SprintInvitation { get; set; }
    }
}