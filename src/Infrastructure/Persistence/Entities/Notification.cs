namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Class define Notification table attributes
    /// </summary>
    public abstract class Notification : BaseEntity
    {
        public int? SenderId { get; set; }
        public int ReceiverId { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }

    public class SprintNotification : Notification
    {
        public int Id { get; set; }
        public SprintNotificaitonType Type { get; set; }
        public int SprintId { get; set; }
        public int? UpdatorId { get; set; }
        public virtual User SprintUpdateBy { get; set; }
        public virtual Sprint Sprint { get; set; }
    }

    public class FriendNoticiation : Notification
    {
        public int Id { get; set; }
        public FriendNoticiationType Type { get; set; }
        public string Status { get; set; }
        public int? RequesterId { get; set; }
        public int? AccepterId { get; set; }
        public virtual User Requester { get; }
        public virtual User Accepter { get; }
    }

    public class AchievementNoticiation : Notification
    {
        public int Id { get; set; }
    }

    public enum NotificationType
    {
        SprintNotification,
        FriendNotification,
        AchivementNotification,
    }

    public enum SprintNotificaitonType
    {
        InvitationRequest,
        InvitationAccept,
        InvitationDecline,
        TimeReminderBeforeStart,
        TimeReminderStarted,
        TimeReminderExpired,
        Edit,
        Remove,
    }

    public enum FriendNoticiationType
    {
        Request,
        Accepet,
        Decline,
    }
}