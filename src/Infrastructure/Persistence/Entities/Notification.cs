namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;

    /// <summary>
    /// Class define Notification table attributes
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or set unique id for notificaiton
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or set type of notification
        /// </summary>
        public NotificationType NotiticationType { get; set; }

        /// <summary>
        /// Gets or set id reference for notificaiton sender
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Gets or set id for notification receiver
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// Gets or set sprint id if available
        /// </summary>
        public int? SprintInviteId { get; set; }

        /// <summary>
        /// Gets or sets achievement id if available
        /// </summary>
        /// <value></value>
        public int? AchievementId { get; set; }

        /// <summary>
        /// Gets or set send time of the notification
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// Gets or set notificaiton read or not
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or set reference for notificaiton sender
        /// </summary>
        public virtual User Sender { get; set; }

        /// <summary>
        /// Gets or set reference for notification receiver
        /// </summary>
        public virtual User Receiver { get; set; }

        /// <summary>
        /// Gets or set reference for sprint
        /// </summary>
        public virtual SprintInvite SprintInvite { get; set; }

        /// <summary>
        /// Gets or set reference for achievement
        /// </summary>
        public virtual Achievement Achievement { get; set; }
    }
}