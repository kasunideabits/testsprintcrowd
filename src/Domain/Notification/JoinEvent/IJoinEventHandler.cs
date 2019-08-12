namespace SprintCrowd.BackEnd.Domain.Notification.JoinEvent
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for Join event
    /// </summary>
    public interface IJoinEventHandler
    {
        /// <summary>
        /// Execute background task for send notifcations and related work
        /// </summary>
        /// <param name="joinEvent"><see cref="JoinEvent"/></param>
        Task Execute(JoinEvent joinEvent);
    }
}