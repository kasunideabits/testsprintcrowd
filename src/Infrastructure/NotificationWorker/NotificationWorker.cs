namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    using System;
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

        /// <summary>
        /// Schedule notification job in backgorund. to run the job implement <see cref="INotificationJob"> job class </see>
        /// </summary>
        /// <param name="message"> message for execture job</param>
        /// <param name="delay"> schedule job delay time</param>
        public string Schedule(object message, TimeSpan delay)
        {
            return BackgroundJob.Schedule<T>(x => x.Run(message), delay);
        }

        /// <summary>
        /// Delete schduled job
        /// </summary>
        /// <param name="jobId">job id</param>
        public void DeleteSchedule(string jobId)
        {
            BackgroundJob.Delete(jobId);
        }
    }
}