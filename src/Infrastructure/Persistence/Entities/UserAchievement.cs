namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;

    public class UserAchievement : BaseEntity
    {
        public int Id { get; set; }

        public AchievementType Type { get; set; }

        public DateTime AchivedOn { get; set; }

        public int Percentage { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; }
    }

    public enum AchievementType
    {
        // When signup for the app first time
        JoinedTheCrowd,

        // Complete first private event
        ThreeIsCrowd,

        // Complete first public event
        CrowdFunded,

        // Complete 10KM
        TenComplete,

        // Complete 20KM
        TwentyComplete,

        // Complete 30KM
        ThirtyComplete,

        // Complete 40KM
        FortyComplete,

        // Complete 50KM
        FiftyComplete,

        // Complete 10 private events
        TenPrivateEventsComplete,

        // Complete 20 private events
        TwentyPrivateEventsComplete,

        // Complete 30 private events
        ThirtyPrivateEventsComplete,

        // Complete 40 private events
        FotyPrivateEventsComplete,

        // Complete 50 private events
        FiftyPrivateEventsComplete,

        // Complete 10 public events
        TenPublicEventsComplete,

        // Complete 20 public events
        TwentyPublicEventsComplete,

        // Complete 30 public events
        ThirtyPublicEventsComplete,

        // Complete 40 public events
        FotyPublicEventsComplete,

        // Complete 50 public events
        FiftyPublicEventsComplete,
    }
}