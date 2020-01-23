namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint;

    /// <summary>
    /// class for exectue background notification tasks
    /// </summary>
    public class NotificationClient : INotificationClient
    {
        /// <summary>
        /// Sprint notification
        /// </summary>
        public ISprintNotificationJobs SprintNotificationJobs
        {
            get
            {
                return new SprintNotificationJobs();
            }
        }

        /// <summary>
        ///  Sprint notification reminder
        /// </summary>
        public ISprintNotificationReminderJobs NotificationReminderJobs
        {
            get
            {
                return new SprintNotificationReminderJobs();
            }
        }

        /// <summary>
        ///  Achievement notfication jobs
        /// </summary>
        public IAchievemenNotificationJobs AchievemenNotificationJobs
        {
            get
            {
                return new AchievemenNotificationJobs();
            }
        }
    }
}