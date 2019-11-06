namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint
{
    using System;
    using src.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;

    /// <summary>
    /// Available sprint notificaitons
    /// </summary>
    public class SprintNotificationJobs : ISprintNotificationJobs
    {
        /// <summary>
        /// Sprint invite notifications
        /// </summary>
        public void SprintInvite(int sprintId, int iniviteId, int inviteeId)
        {
            var message = new InviteSprint(sprintId, iniviteId, inviteeId);
            new NotificationWorker<SprintInvite>().Invoke(message);
        }

        /// <summary>
        /// Sprint mark attendance
        /// </summary>
        public void SprintMarkAttendace(int sprintId, int userId, string name, string profilePicture, string country, string countryCode, string city, string colorCode)
        {
            var message = new MarkAttendance(sprintId, userId, name, profilePicture, country, countryCode, city, colorCode);
            new NotificationWorker<SprintMarkAttendance>().Invoke(message);
        }

        /// <summary>
        /// Sprint exit
        /// </summary>
        public void SprintExit(
            int sprintId,
            string sprintName,
            int distance,
            DateTime startTime,
            int numberOfParticipant,
            SprintStatus sprintStatus,
            SprintType sprintType,
            int userId,
            string name,
            string profilePicture,
            string code,
            string city,
            string country,
            string countryCode)
        {
            var message = new ExitSprint(
                sprintId, sprintName, distance, startTime, numberOfParticipant, sprintStatus, sprintType,
                userId, name, profilePicture, code, city, country, countryCode);
            new NotificationWorker<SprintExit>().Invoke(message);
        }

        /// <summary>
        /// Sprint join
        /// </summary>
        public void SprintJoin(int sprintId, string sprintName, SprintType sprintType, int userId, string name, string profilePicture, bool accept)
        {
            var message = new JoinSprint(sprintId, sprintName, sprintType, userId, name, profilePicture, accept);
            new NotificationWorker<SprintJoin>().Invoke(message);
        }
    }
}