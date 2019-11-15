namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using static SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs.SprintNotificationJobBase;

    public class SprintNotificationPersistence : IDisposable
    {
        public SprintNotificationPersistence(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventInfo"></param>
        /// <param name="user"></param>
        /// <param name="receiverIds"></param>
        /// <param name="notificationType"></param>
        /// <returns></returns>
        public int AddToDatabase(
            NotificationSprintInfo eventInfo,
            int senderId,
            List<int> receiverIds,
            SprintNotificaitonType notificationType)
        {
            List<UserNotification> userNotifications = new List<UserNotification>();
            var sprintNotification = new SprintNotification
            {
                SprintNotificationType = notificationType,
                UpdatorId = senderId,
                SprintId = eventInfo.Id,
                SprintName = eventInfo.Name,
                Distance = eventInfo.Distance,
                StartDateTime = eventInfo.StartTime,
                SprintType = eventInfo.SprintType,
                SprintStatus = eventInfo.SprintStatus,
                NumberOfParticipants = eventInfo.NumberOfParticipants
            };
            var notification = this.Context.Notification.Add(sprintNotification);
            receiverIds.ForEach(receiverId =>
            {
                userNotifications.Add(new UserNotification
                {
                    SenderId = senderId,
                        ReceiverId = receiverId,
                        NotificationId = notification.Entity.Id,
                });
            });
            if (userNotifications.Count > 0)
            {
                this.Context.UserNotification.AddRange(userNotifications);
            }
            return notification.Entity.Id;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetParticipant(int userId) => this.Context.User.FirstOrDefault(u => u.Id == userId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        public Infrastructure.Persistence.Entities.Sprint GetSprint(int sprintId) => this.Context.Sprint.FirstOrDefault(s => s.Id == sprintId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        public User GetCreator(int sprintId)
        {
            return this.Context.Sprint
                .Include(s => s.CreatedBy)
                .Where(s => s.Id == sprintId).Select(s => s.CreatedBy).FirstOrDefault();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<int> SprintParticipantIds(Expression<Func<SprintParticipant, bool>> query)
        {
            return this.Context.SprintParticipant
                .Where(query)
                .Select(s => s.UserId)
                .ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        public List<int> GetFriendIdsInSprint(int userId, int sprintId)
        {
            List<int> ids = new List<int>();
            var ids1 = this.Context.SprintParticipant
                .Where(s => s.SprintId == sprintId)
                .Join(this.Context.Frineds,
                    p => p.UserId,
                    f => f.SharedUserId,
                    ((p, f) =>
                        new { UserId = p.UserId, FriendId = f.AcceptedUserId }))
                .Where(s => s.UserId == userId)
                .Select(s => s.FriendId)
                .ToList();
            var ids2 = this.Context.SprintParticipant
                .Where(s => s.SprintId == sprintId)
                .Join(this.Context.Frineds,
                    p => p.UserId,
                    f => f.AcceptedUserId,
                    ((p, f) =>
                        new { UserId = p.UserId, FriendId = f.SharedUserId }))
                .Where(s => s.UserId == userId)
                .Select(s => s.FriendId)
                .ToList();
            ids.AddRange(ids1);
            ids.AddRange(ids2);
            return ids;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        public void RemoveOldNotificaiton(int sprintId)
        {
            var toDeleteNotifications = this.Context.SprintNotifications.Where(n => n.SprintId == sprintId && n.SprintNotificationType != SprintNotificaitonType.Remove);
            this.Context.RemoveRange(toDeleteNotifications);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<string> GetTokens(List<int> userIds)
        {
            return this.Context.FirebaseToken.Where(f => userIds.Contains(f.User.Id)).Select(f => f.Token).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprintId"></param>
        /// <param name="sprintName"></param>
        /// <param name="distance"></param>
        /// <param name="startTime"></param>
        public void UpdateSprintNotification(int sprintId, string sprintName, int distance, DateTime startTime)
        {
            List<SprintNotification> existingNotification = this.Context.SprintNotifications.Where(s => s.SprintId == sprintId).ToList();
            existingNotification.ForEach(n =>
            {
                n.SprintName = sprintName;
                n.Distance = distance;
                n.StartDateTime = startTime;
            });
            this.Context.SprintNotifications.UpdateRange(existingNotification);
        }

        /// <summary>
        ///
        /// </summary>
        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}