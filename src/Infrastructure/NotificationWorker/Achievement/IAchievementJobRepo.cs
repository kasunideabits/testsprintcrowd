namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement
{
    using System.Collections.Generic;
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public interface IAchievementJobRepo
    {
        List<string> GetTokens(int userId);
        int AddNotification(Persistence.Entities.AchievementType type, DateTime achivedOn);
        void AddUserNotification(int creatorId, int receiverId, int notificationId);
        User GetSystemUser();
        User GetUser(int userId);
        void SaveChanges();
    }
}