namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Repo
{
    using System.Collections.Generic;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public interface ISprintReminderRepo
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        /// <returns></returns>
        Sprint GetSprint(int sprintId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        /// <param name="notificationType"></param>
        /// <returns></returns>
        Dictionary<string, List<int>> GetParticipantIdsByLangugage(int sprintId, SprintNotificaitonType notificationType);

        /// <summary>
        ///Get Tokens
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        List<string> GetTokens(List<int> userIds);

        /// <summary>
        /// Get Token
        /// </summary>
        /// <param name="creatorId"></param>
        /// <returns></returns>
        List<string> GetToken(int creatorId);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        User GetSystemUser();

        /// <summary>
        ///
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="sprintId"></param>
        /// <param name="sprintName"></param>
        /// <param name="distance"></param>
        /// <param name="sprintType"></param>
        /// <param name="status"></param>
        /// <param name="numberOfParticipants"></param>
        /// <param name="startTime"></param>
        /// <param name="creatorId"></param>
        /// <returns></returns>
        int AddNotification(SprintNotificaitonType notificationType, int sprintId,
            string sprintName, int distance, SprintType sprintType, SprintStatus status,
            int numberOfParticipants, DateTime startTime, int creatorId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="participantIds"></param>
        /// <param name="creatorId"></param>
        /// <param name="notificationId"></param>
        void AddUserNotification(List<int> participantIds, int creatorId, int notificationId);

        /// <summary>
        ///
        /// </summary>
        void SaveChanges();
    }
}