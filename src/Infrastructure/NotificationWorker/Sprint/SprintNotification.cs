namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint
{
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs;

    /// <summary>
    /// Available sprint notificaitons
    /// </summary>
    public class SprintNotification : ISprintNotification
    {
        /// <summary>
        /// Sprint invite notifications
        /// </summary>
        public void SprintInvite()
        {
            new NotificationWorker<SprintInvite>().Invoke();
        }
    }
}