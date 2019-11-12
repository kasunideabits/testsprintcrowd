namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System.Collections.Generic;
    using System;
    using SprintCrowd.BackEnd.Application;

    public abstract class Notification : BaseEntity
    {
        public int Id { get; set; }
        public virtual ICollection<UserNotification> UserNotification { get; set; }
    }

    public class SprintNotification : Notification
    {
        public SprintNotificaitonType SprintNotificationType { get; set; }
        public int? UpdatorId { get; set; }
        public int SprintId { get; set; }
        public string SprintName { get; set; }
        public int Distance { get; set; }
        public DateTime StartDateTime { get; set; }
        public SprintType SprintType { get; set; }
        public SprintStatus SprintStatus { get; set; }
        public int NumberOfParticipants { get; set; }
    }

    public class FriendNoticiation : Notification
    {
        public FriendNoticiationType Type { get; set; }
        public string Status { get; set; }
        public int? RequesterId { get; set; }
        public int? AccepterId { get; set; }
        public virtual User Requester { get; }
        public virtual User Accepter { get; }
    }

    public class AchievementNoticiation : Notification
    {
        public int AchievementId { get; set; }
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
        FriendJoin,
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