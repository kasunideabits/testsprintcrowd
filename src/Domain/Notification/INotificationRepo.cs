using System.Collections.Generic;
using System.Threading.Tasks;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.Notification
{
    public interface INotificationRepo
    {
        Task<List<Infrastructure.Persistence.Entities.Notification>> GetNotifications(int userId);

    }
}