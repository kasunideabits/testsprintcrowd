namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Handle notificatio related db works
    /// </summary>
    public class NotificationRepo : INotificationRepo
    {
        /// <summary>
        /// Initialize notification repo class
        /// </summary>
        /// <param name="context">database context</param>
        public NotificationRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        /// <summary>
        /// Get notificaitons related to given user id
        /// </summary>
        /// <param name="userId">user id to lookup</param>
        /// <returns>notificaitons realted to user </returns>
        public async Task<List<Infrastructure.Persistence.Entities.Notification>> GetNotifications(int userId)
        {
            return await this.Context.Notification
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Include(n => n.SprintInvite).ThenInclude(s => s.Sprint)
                .Include(n => n.Achievement)
                .Where(n => n.ReceiverId == userId || n.SenderId == userId)
                .ToListAsync();
        }
    }
}