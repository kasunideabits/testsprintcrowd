using System.Collections.Generic;
using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Notification
{
    /// <summary>
    /// Interface for notificaiton service
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Get notificaitons related to given user id
        /// </summary>
        /// <param name="userId">user id to lookup</param>
        /// <returns><see cref="NotificationList">notificaitons realted to user </see></returns>
        Task<NotificationList> GetNotifications(int userId);
    }
}