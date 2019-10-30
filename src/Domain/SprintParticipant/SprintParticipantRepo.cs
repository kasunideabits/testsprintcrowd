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
        /// Set participant stage to <see cref="ParticipantStage">QUIT</see>
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userId">user id which leaving the event</param>
        public async Task<ParticipantInfo> ExitSprint(int sprintId, int userId)
        {
            var participant = await this.Context.SprintParticipant
                .Include(sp => sp.Sprint)
                .Include(sp => sp.User)
                .FirstOrDefaultAsync(sp => sp.User.Id == userId && sp.Sprint.Id == sprintId);
            if (participant != null)
            {

                participant.Stage = ParticipantStage.QUIT;
                this.Context.Update(participant);
                return new ParticipantInfo(
                    userId,
                    participant.User.Name,
                    participant.User.ProfilePicture,
                    participant.User.Code,
                    participant.User.ColorCode,
                    sprintId,
                    participant.Sprint.Name);
            }
            else
            {
                throw new Application.ApplicationException(ExitFaildReason.UserOrSprintNotMatch);
            }
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

        public async Task<User> GetParticipant(int userId)
        {
            return await this.Context.User.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Sprint> GetSprint(int sprintId)
        {
            return await this.Context.Sprint.FirstOrDefaultAsync(u => u.Id == sprintId);
        }

        public IQueryable<Notification> GetNotification(int userId)
        {
            return this.Context.SprintNotifications
                .Include(n => n.Sender)
                .Include(n => n.Receiver)
                .Where(n => n.ReceiverId == userId);
        }

        public async Task JoinSprint(int userId)
        {
            var participant = await this.Context.SprintParticipant.FirstOrDefaultAsync(s => s.UserId == userId);
            participant.Stage = ParticipantStage.JOINED;
            this.Context.Update(participant);
            return;
        }

        public async Task DeleteParticipant(int userId)
        {
            var participant = await this.Context.SprintParticipant.FirstOrDefaultAsync(s => s.UserId == userId);
            participant.Stage = ParticipantStage.DECLINE;
            this.Context.Update(participant);
            return;
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