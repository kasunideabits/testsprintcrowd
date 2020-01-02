namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// class for exectue background notification tasks
    /// </summary>
    public class NotificationClient : INotificationClient
    {
        public NotificationClient(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

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
                return new SprintNotificationReminderJobs(this.Context);
            }
        }
    }
}