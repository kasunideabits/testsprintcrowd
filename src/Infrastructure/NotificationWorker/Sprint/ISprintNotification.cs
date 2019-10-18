namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint
{
    /// <summary>
    /// Sprint notification types interface
    /// </summary>
    public interface ISprintNotification
    {
        /// <summary>
        /// Sprint invite
        /// </summary>
        void SprintInvite();

        /// <summary>
        /// Sprint mark attendance
        /// </summary>
        void SprintMarkAttendace(int sprintId, int userId, string name, string profilePicture, string country, string countryCode, string city, string colorCode);

        /// <summary>
        /// Sprint exit
        /// </summary>
        void SprintExit(int sprintId, string sprintName, int userId, string name, string profilePicture);
    }
}