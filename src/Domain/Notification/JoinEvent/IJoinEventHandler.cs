using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Notification.JoinEvent
{
    /// <summary>
    /// Interface for Join event
    /// </summary>
    public interface IJoinEventHandler
    {
        /// <summary>
        /// Execute background task for send notifcations and related work
        /// </summary>
        /// <param name="joinEvent"><see cref="JoinEvent"/></param>
        /// <returns></returns>
        Task Execute(JoinEvent joinEvent);
    }
}