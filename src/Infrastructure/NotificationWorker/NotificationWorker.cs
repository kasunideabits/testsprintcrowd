namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    using Hangfire;

    /// <summary>
    /// Notification worker which responsible for invoke notification job
    /// </summary>
    public class NotificationWorker<T> : INotificationWorker<T> where T : INotificationJob
    {
        /// <summary>
        /// Invoke notification in background, to run the job implement <see cref="INotificationJob"> job class </see>
        /// </summary>
        public virtual void Invoke()
        {
            BackgroundJob.Enqueue<T>(x => x.Run(null));
        }

        /// <summary>
        /// Invoke notification in background, to run the job implement <see cref="INotificationJob"> job class </see>
        /// </summary>
        public virtual void Invoke(object message)
        {
            BackgroundJob.Enqueue<T>(x => x.Run(message));
        }
    }
}