using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Notification.ExitEvent
{
    /// <summary>
    /// Interface for Exit event
    /// </summary>
    public interface IExitEventHandler
    {
        /// <summary>
        /// Execute background task for send notifcations and related work
        /// </summary>
        /// <param name="exitEvent"><see cref="ExitEvent"/></param>
        /// <returns></returns>
        Task Execute(ExitEvent exitEvent);
    }
}