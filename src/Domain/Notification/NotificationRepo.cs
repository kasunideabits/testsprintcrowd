namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    public class NotificationRepo : INotificationRepo
    {
        public NotificationRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        public async Task<List<Infrastructure.Persistence.Entities.Notification>> GetNotifications(int userId)
        {
            return await this.Context.Notification
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.Sprint)
                .Include(n => n.Achievement)
                .Where(n => n.ReceiverId == userId || n.SenderId == userId)
                .ToListAsync();
        }
    }
}