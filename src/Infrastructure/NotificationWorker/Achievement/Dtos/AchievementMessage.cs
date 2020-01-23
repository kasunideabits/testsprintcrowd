namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement.Dtos
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class AchievementMessage
    {
        public AchievementMessage(int userId, int type, DateTime achievedOn)
        {
            this.UserId = userId;
            this.Type = type;
            this.AchievedOn = achievedOn;
        }
        public int UserId { get; }
        public int Type { get; }
        public DateTime AchievedOn { get; }
    }
}