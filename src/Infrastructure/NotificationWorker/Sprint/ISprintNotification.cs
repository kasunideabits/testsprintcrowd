namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint
{
    /// <summary>
    /// Sprint notification types interface
    /// </summary>
    public interface ISprintNotification
    {
        /// <summary>
        /// Sprint invite
        /// </summary>
        void SprintInvite();
    }
}