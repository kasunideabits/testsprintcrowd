namespace SprintCrowd.Domain.Achievement
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class AchievementDto
    {
        public AchievementDto(UserAchievement achievement)
        {
            this.Type = achievement.Type;
            this.AchievedOn = achievement.AchivedOn;
        }
        public AchievementType Type { get; }
        public DateTime AchievedOn { get; }
    }
}