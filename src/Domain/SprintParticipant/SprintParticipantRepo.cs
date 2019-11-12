namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// Implements ISprintParticipantRepo interface for hanle sprint participants
    /// </summary>
    public class SprintParticipantRepo : ISprintParticipantRepo
    {
        /// <summary>
        /// Initalize SprintParticipantRepo class
        /// </summary>
        /// <param name="context">dabase context</param>
        public SprintParticipantRepo(ScrowdDbContext context)
        {
            this.Context = context;
        }

        private ScrowdDbContext Context { get; }

        /// <summary>
        /// Mark attendece for given sprint
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for mark attendance</param>
        /// <returns>User details</returns>
        public async Task<User> MarkAttendence(int sprintId, int userId)
        {
            SprintParticipant paritipant = await this.Context.SprintParticipant
                .FirstOrDefaultAsync(s => s.Sprint.Id == sprintId && s.User.Id == userId);
            if (paritipant == null)
            {
                throw new Application.ApplicationException("Entry not found");

            }
            if (paritipant.Stage == ParticipantStage.JOINED)
            {
                paritipant.Stage = ParticipantStage.MARKED_ATTENDENCE;
                this.Context.SprintParticipant.Update(paritipant);
                return await this.Context.User.FirstOrDefaultAsync(u => u.Id == userId);
            }
            else if (paritipant.Stage == ParticipantStage.MARKED_ATTENDENCE)
            {
                throw new Application.ApplicationException("Already marked attendance");
            }
            else
            {
                throw new Application.ApplicationException("Join before marked attendance");
            }
        }

        /// <summary>
        /// User join for an event
        /// </summary>
        /// <param name="sprintId">sprint id for join</param>
        /// <param name="userId">user id for who join</param>
        /// <returns>joined user details</returns>
        public async Task<SprintParticipant> AddSprintParticipant(int sprintId, int userId)
        {
            SprintParticipant participant = new SprintParticipant()
            {
                UserId = userId,
                SprintId = sprintId,
                Stage = ParticipantStage.JOINED,
            };
            var result = await this.Context.SprintParticipant.AddAsync(participant);
            return result.Entity;
        }

        /// <summary>
        /// Get all pariticipant with given stage <see cref="SprintParticipant"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="stage">filter with stage</param>
        /// <returns><see cref="SprintParticipant"> list of participant info</see></returns>
        public async Task<List<SprintParticipant>> GetParticipants(int sprintId, ParticipantStage stage)
        {
            return await this.Context.SprintParticipant
                .Include(p => p.User)
                .Include(p => p.Sprint)
                .Where(p => p.SprintId == sprintId && p.Stage == stage)
                .ToListAsync();
        }

        /// <summary>
        /// Check user exist in sprint
        /// </summary>
        /// <param name="sprintId">sprint id for check</param>
        /// <param name="userId">user id for check</param>
        /// <returns><see cref="SprintParticipant"> participant info</see></returns>
        public async Task<SprintParticipant> CheckSprintParticipant(int sprintId, int userId)
        {
            SprintParticipant result = await this.Context.SprintParticipant
                .FirstOrDefaultAsync(sp => sp.SprintId == sprintId && sp.UserId == userId);
            return result;
        }

        /// <summary>
        /// Filter sprint pariticipant detials with sprint and user details with given query
        /// </summary>
        /// <param name="query">query to filter record</param>
        /// <returns><see cref="SprintParticipant"> entity</see>/<returns>
        public async Task<SprintParticipant> Get(Expression<Func<SprintParticipant, bool>> query)
        {
            return await this.Context.SprintParticipant
                .Include(s => s.User)
                .Include(s => s.Sprint)
                .ThenInclude(s => s.CreatedBy)
                .FirstOrDefaultAsync(query);
        }

        /// <summary>
        /// Get all sprints paritipant details with given query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>all pariticpant details</returns>
        public IEnumerable<SprintParticipant> GetAll(Expression<Func<SprintParticipant, bool>> query)
        {
            return this.Context.SprintParticipant
                .Include(s => s.User)
                .Include(s => s.Sprint)
                .ThenInclude(s => s.CreatedBy)
                .Where(query);
        }

        /// <summary>
        /// Add pariticipant to sprint
        /// </summary>
        /// <param name="sprintId">sprint id </param>
        /// <param name="userId">user id</param>
        /// <returns>participant entity</returns>
        public async Task<SprintParticipant> AddParticipant(int sprintId, int userId)
        {
            SprintParticipant pariticipant = new SprintParticipant()
            {
                UserId = userId,
                SprintId = sprintId,
                Stage = ParticipantStage.PENDING,
            };
            var result = await this.Context.AddAsync(pariticipant);
            return result.Entity;
        }

        /// <summary>
        /// Get pariticipant with given user id
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>User record</returns>
        public async Task<User> GetParticipant(int userId)
        {
            return await this.Context.User.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Get sprint details with given sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to fetch</param>
        /// <returns>sprint record</returns>
        public async Task<Sprint> GetSprint(int sprintId)
        {
            return await this.Context.Sprint.FirstOrDefaultAsync(u => u.Id == sprintId);
        }

        /// <summary>
        /// Get notifications for given user id
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>notificaitons</returns>
        public IQueryable<NotificationInfo> GetNotification(int userId)
        {
            return this.Context.UserNotification
                .Where(u => u.ReceiverId == userId)
                .Join(this.Context.Notification,
                    u => u.NotificationId,
                    n => n.Id,
                    (u, n) => new NotificationInfo { Sender = u.Sender, Receiver = u.Receiver, Notification = n });
        }

        /// <summary>
        /// Join participant to given sprint
        /// </summary>
        /// <param name="userId">user id who want to participate</param>
        /// <param name="sprintId">sprint id to join</param>
        public async Task JoinSprint(int userId, int sprintId)
        {
            var participant = await this.Context.SprintParticipant.FirstOrDefaultAsync(s => s.UserId == userId && s.SprintId == sprintId);
            if (participant != null)
            {
                participant.Stage = ParticipantStage.JOINED;
                this.Context.Update(participant);
            }
        }

        /// <summary>
        /// Delete pariticipant form sprint
        /// </summary>
        /// <param name="userId">user id for delete</param>
        /// <param name="sprintId">sprint id for delete</param>
        public async Task DeleteParticipant(int userId, int sprintId)
        {
            var participant = await this.Context.SprintParticipant.FirstOrDefaultAsync(s => s.UserId == userId && s.SprintId == sprintId);
            if (participant != null)
            {
                this.Context.Remove(participant);
            }
            return;
        }

        // <summary>
        /// Get pariticipant count in given sprint id
        /// </summary>
        /// <param name="sprintId">sprint it to count</param>
        /// <returns>count for participants</returns>
        public int GetParticipantCount(int sprintId)
        {
            return this.Context.SprintParticipant.Where(s => s.SprintId == sprintId && s.Stage != ParticipantStage.PENDING).Count();
        }

        /// <summary>
        /// Remove particiapnt
        /// </summary>
        /// <param name="participant">participant record</param>
        public void RemoveParticipant(SprintParticipant participant)
        {
            this.Context.SprintParticipant.Remove(participant);
        }

        /// <summary>
        /// Get friends
        /// </summary>
        /// <param name="userId">user id to check friends</param>
        /// <returns>list of friends</returns>
        public IEnumerable<Friend> GetFriends(int userId)
        {
            return this.Context.Frineds
                .Include(f => f.AcceptedUser)
                .Include(f => f.SharedUser)
                .Where(f => f.AcceptedUserId == userId || f.SharedUserId == userId);
        }

        /// <summary>
        /// Remove notification
        /// </summary>
        /// <param name="notificationId">notificaiton id</param>
        public async Task RemoveNotification(int notificationId)
        {
            var notification = await this.Context.Notification.Where(n => n.Id == notificationId).FirstOrDefaultAsync();
            if (notification != null)
            {
                this.Context.Remove(notification);
            }
            return;
        }

        /// <summary>
        /// generic method to find with include
        /// </summary>
        /// <typeparam name="T">any database entity</typeparam>
        public async Task<T> FindWithInclude<T>(Expression<Func<T, bool>> predicate, params string [] includeProperties)where T : class, new()
        {
            IQueryable<T> query = this.Context.Set<T>();
            foreach (var includePropertie in includeProperties)
            {
                query = query.Include(includePropertie);
            }
            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }

    }
}