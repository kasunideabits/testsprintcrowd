using System;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    /// <summary>
    /// Notification worker which responsible for invoke notification job
    /// </summary>
    public interface INotificationWorker<T> where T : INotificationJob
    {
        /// <summary>
        /// Invoke notification in background, to run the job implement <see cref="INotificationJob"> job class </see>
        /// </summary>
        void Invoke();

        /// <summary>
        /// Invoke notification in background, to run the job implement <see cref="INotificationJob"> job class </see>
        /// </summary>
        /// <param name="message"> optional message object for execute job</param>
        void Invoke(object message);

        /// <summary>
        /// Schedule notification job in backgorund. to run the job implement <see cref="INotificationJob"> job class </see>
        /// </summary>
        /// <param name="message"> message for execture job</param>
        /// <param name="delay"> schedule job delay time</param>
        void Schedule(object message, TimeSpan delay);
    }
}