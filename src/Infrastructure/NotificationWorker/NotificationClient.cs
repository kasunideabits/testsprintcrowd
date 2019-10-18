using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    /// <summary>
    /// class for exectue background notification tasks
    /// </summary>
    public class NotificationClient : INotificationClient
    {
        /// <summary>
        /// Sprint notification
        /// </summary>
        /// <value></value>
        public ISprintNotification SprintNotification
        {
            get
            {
                return new SprintNotification();
            }
        }
    }
}