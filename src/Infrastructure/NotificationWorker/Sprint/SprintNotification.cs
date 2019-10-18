namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint
{
    using src.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;

    /// <summary>
    /// Available sprint notificaitons
    /// </summary>
    public class SprintNotification : ISprintNotification
    {
        /// <summary>
        /// Sprint invite notifications
        /// </summary>
        public void SprintInvite()
        {
            new NotificationWorker<SprintInvite>().Invoke();
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
        public void SprintExit(int sprintId, string sprintName, int userId, string name, string profilePicture)
        {
            var message = new ExitSprint(sprintId, sprintName, userId, name, profilePicture);
            new NotificationWorker<SprintExit>().Invoke(message);
        }

        /// <summary>
        /// Sprint join
        /// </summary>
        public void SprintJoin(int sprintId, string sprintName, int userId, string name, string profilePicture)
        {
            var message = new JoinSprint(sprintId, sprintName, userId, name, profilePicture);
            new NotificationWorker<SprintJoin>().Invoke(message);
        }
    }
}