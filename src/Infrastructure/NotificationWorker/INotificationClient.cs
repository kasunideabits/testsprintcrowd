using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders;
using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    /// <summary>
    /// public interface for execute notifications. define notification types in here
    /// </summary>
    public interface INotificationClient
    {
        /// <summary>
        /// Sprint notification type
        /// </summary>
        ISprintNotificationJobs SprintNotificationJobs { get; }

        /// <summary>
        ///  Sprint notification reminder
        /// </summary>
        ISprintNotificationReminderJobs NotificationReminderJobs { get; }
    }
}