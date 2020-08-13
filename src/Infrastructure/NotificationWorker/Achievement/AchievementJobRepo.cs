using System;
using System.Collections.Generic;
using System.Linq;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement
{
    public class AchievementJobRepo : IAchievementJobRepo
    {
        public AchievementJobRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        public int AddNotification(Persistence.Entities.AchievementType type, DateTime achivedOn)
        {
            var achievementNotification = new AchievementNoticiation()
            {
                AchievementType = type,
                AchievedOn = achivedOn,
            };
            var notification = this.Context.Notification.Add(achievementNotification);
            return notification.Entity.Id;
        }

        public void AddUserNotification(int creatorId, int receiverId, int notificationId)
        {
            var userNotification = new UserNotification
            {
                SenderId = creatorId,
                ReceiverId = receiverId,
                NotificationId = notificationId,
                BadgeValue = 1,
            };
            this.Context.UserNotification.Add(userNotification);
        }

        public User GetSystemUser()
        {
            var result = this.Context.User.FirstOrDefault(u => u.UserType == (int)UserType.SystemUser);
            return result;
        }

        public User GetUser(int userId)
        {
            var result = this.Context.User.FirstOrDefault(u => u.Id == userId);
            return result;
        }

        public List<string> GetTokens(int userId)
        {
            return this.Context.FirebaseToken.Where(f => f.User.Id == userId).Select(f => f.Token).ToList();
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}