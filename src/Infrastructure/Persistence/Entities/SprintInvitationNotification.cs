namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    public class SprintInvitationNotification
    {
        public int Id { get; set; }

        public int SprintId { get; set; }

        public int NotificationId { get; set; }

        public virtual Sprint Sprint { get; set; }

        public virtual Notifications Notification { get; set; }

    }
}