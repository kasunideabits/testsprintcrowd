namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Repo
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    internal class SprintReminderRepo : ISprintReminderRepo
    {
        public SprintReminderRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        /// <param name="notificationType"></param>
        /// <returns></returns>
        public Dictionary<string, List<int>> GetParticipantIdsByLangugage(int sprintId, SprintNotificaitonType notificationType)
        {
            var result = this.Context.SprintParticipant
                .Join(this.Context.UserNotificationReminders,
                    s => s.UserId,
                    u => u.UserId,
                    (s, u) => new { User = s, Reminders = u })
                .Where(s =>
                    s.User.SprintId == sprintId &&
                    s.User.User.UserState == Application.UserState.Active &&
                    (s.User.Stage == ParticipantStage.JOINED || s.User.Stage == ParticipantStage.MARKED_ATTENDENCE) &&
                    (
                        (s.Reminders.TwentyFourH && SprintNotificaitonType.TimeReminderBeforeStart == notificationType) ||
                        (s.Reminders.OneH && SprintNotificaitonType.TimeReminderOneHourBefore == notificationType) ||
                        (s.Reminders.FiftyM && SprintNotificaitonType.TimeReminderBeforFiftyM == notificationType) ||
                        (s.Reminders.EventStart && SprintNotificaitonType.TimeReminderStarted == notificationType) ||
                        (s.Reminders.FinalCall && SprintNotificaitonType.TimeReminderFinalCall == notificationType) ||
                        (SprintNotificaitonType.TimeReminderExpired == notificationType)
                    ))
                .Select(s => new { Language = s.User.User.LanguagePreference, UserId = s.User.UserId })
                .GroupBy(s => s.Language,
                    s => s.UserId,
                    (key, g) => new { Language = key, UserId = g.ToList() })
                .ToDictionary(s => s.Language, s => s.UserId);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        public Sprint GetSprint(int sprintId)
        {
            var result = this.Context.Sprint.FirstOrDefault(s => s.Id == sprintId);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<string> GetTokens(List<int> userIds)
        {
            var result = this.Context.FirebaseToken.Where(f => userIds.Contains(f.User.Id)).Select(f => f.Token).ToList();
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public User GetSystemUser()
        {
            var result = this.Context.User.FirstOrDefault(u => u.UserType == (int)UserType.SystemUser);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        /// <param name="sprintName"></param>
        /// <param name="distance"></param>
        /// <param name="sprintType"></param>
        /// <param name="status"></param>
        /// <param name="numberOfParticipants"></param>
        /// <param name="startTime"></param>
        /// <param name="creatorId"></param>
        /// <returns></returns>
        public int AddNotification(int sprintId, string sprintName, int distance, SprintType sprintType,
            SprintStatus status, int numberOfParticipants, DateTime startTime, int creatorId)
        {

            var sprintNotification = new SprintNotification()
            {
                SprintNotificationType = SprintNotificaitonType.Edit,
                UpdatorId = creatorId,
                SprintId = sprintId,
                SprintName = sprintName,
                Distance = distance,
                StartDateTime = startTime,
                SprintType = sprintType,
                SprintStatus = status,
                NumberOfParticipants = numberOfParticipants
            };
            var notification = this.Context.Notification.Add(sprintNotification);

            return notification.Entity.Id;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="participantIds"></param>
        /// <param name="creatorId"></param>
        /// <param name="notificationId"></param>
        public void AddUserNotification(List<int> participantIds, int creatorId, int notificationId)
        {
            List<UserNotification> userNotifications = new List<UserNotification>();
            participantIds.ForEach(id =>
            {
                userNotifications.Add(new UserNotification
                {
                    SenderId = creatorId,
                        ReceiverId = id,
                        NotificationId = notificationId,
                });

            });
            this.Context.UserNotification.AddRange(userNotifications);
        }

        /// <summary>
        ///
        /// </summary>
        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }

}