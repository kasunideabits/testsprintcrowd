using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    /// <summary>
    /// Notification job interface, implement this interface to run background job
    /// </summary>
    public interface INotificationJob
    {
        /// <summary>
        /// Run the job. implement the job logic in here
        /// </summary>
        /// <param name="message"> optional message object for execute job</param>
        void Run(object message = null);
    }
}