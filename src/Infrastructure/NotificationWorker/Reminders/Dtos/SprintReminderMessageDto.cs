namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Dtos
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    ///  Sprint time reminder dto
    /// </summary>
    public class SprintReminderMessageDto
    {
        /// <summary>
        /// Initialize SprintReminderMessageDto class
        /// </summary>
        /// <param name="sprintId">sprint id for scheule task</param>
        /// <param name="sprintName">sprint name</param>
        /// <param name="notificationType">notification reminder time indication</param>
        public SprintReminderMessageDto(int sprintId, string sprintName, SprintNotificaitonType notificationType)
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