using System.Collections.Generic;
using System.Threading.Tasks;

namespace SprintCrowd.BackEnd.Domain.Notification
{
    public interface INotificationService
    {
        Task<NotificationList> GetNotifications(int userId);
    }
}