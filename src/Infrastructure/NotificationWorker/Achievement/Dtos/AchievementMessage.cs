namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement.Dtos
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class AchievementMessage
    {
        public AchievementMessage(int userId, AchievementType type, DateTime achievedOn)
        {
            this.UserId = userId;
            this.AchievementType = type;
            this.AchievedOn = achievedOn;
        }
        public int UserId { get; }
        public AchievementType AchievementType { get; }
        public DateTime AchievedOn { get; }
    }
}