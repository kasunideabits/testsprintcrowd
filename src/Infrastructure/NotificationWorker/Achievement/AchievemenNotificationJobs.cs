namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement.Jobs;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class AchievemenNotificationJobs : IAchievemenNotificationJobs
    {
        public void Achieved(int userId, AchievementType type, DateTime achievedOn)
        {
            var message = new AchievementMessage(userId, type, achievedOn);
            new NotificationWorker<AchievementJob>().Invoke(message);
        }
    }

}