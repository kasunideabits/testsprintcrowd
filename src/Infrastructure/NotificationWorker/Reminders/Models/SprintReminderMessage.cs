namespace SprintCrowd.Infrastructure.NotificationWorker.Reminders.Models
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Sprint reminder message instance
    /// </summary>
    internal class SprintReminderMessage
    {
        /// <summary>
        /// Initialize SprintReminderMessage class
        /// </summary>
        /// <param name="sprintId">sprint id for scheule task</param>
        /// <param name="sprintName">sprint name</param>
        /// <param name="notificationType">notification reminder time indication</param>
        public SprintReminderMessage(int sprintId, string sprintName, SprintNotificaitonType notificationType)
        {
            this.SprintId = sprintId;
            this.SprintName = sprintName;
            this.NotificationType = notificationType;
        }

        /// <summary>
        /// sprint id for scheule task
        /// </summary>
        public int SprintId { get; }

        /// <summary>
        /// sprint name
        /// </summary>
        public string SprintName { get; }

        /// <summary>
        /// notification reminder time indication
        /// </summary>
        public SprintNotificaitonType NotificationType { get; }
    }
}