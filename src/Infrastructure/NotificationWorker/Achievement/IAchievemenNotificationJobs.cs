namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public interface IAchievemenNotificationJobs
    {
        void Achieved(int userId, AchievementType type, DateTime achievedOn);
    }
}