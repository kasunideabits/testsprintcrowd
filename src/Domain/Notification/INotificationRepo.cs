using System.Collections.Generic;
using System.Threading.Tasks;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.Notification
{
    /// <summary>
    /// Repository interface for notification
    /// </summary>
    public interface INotificationRepo
    {
        /// <summary>
        /// Get notificaitons related to given user id
        /// </summary>
        /// <param name="userId">user id to lookup</param>
        /// <returns>notificaitons realted to user </returns>
        Task<List<Infrastructure.Persistence.Entities.Notification>> GetNotifications(int userId);

    }
}