namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    ///  Class define SprintInvitationNotification table attributes
    /// </summary>
    public class SprintInvitationNotification
    {
        /// <summary>
        /// Unique id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sprint id
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// Notification id
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// Sprint reference
        /// </summary>
        public virtual Sprint Sprint { get; set; }

        /// <summary>
        /// Notification reference
        /// </summary>
        public virtual Notifications Notification { get; set; }
    }
}