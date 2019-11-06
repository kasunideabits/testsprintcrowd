namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Interface for sprint participant repository
    /// </summary>
    public interface ISprintParticipantRepo
    {
        /// <summary>
        /// Mark attendece for given sprint
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for mark attendance</param>
        /// <returns>User details</returns>
        Task<User> MarkAttendence(int sprintId, int userId);

        /// <summary>
        /// User join for an event
        /// </summary>
        /// <param name="sprintId">sprint id for join</param>
        /// <param name="userId">user id for who join</param>
        /// <returns>joined user details</returns>
        Task<SprintParticipant> AddSprintParticipant(int sprintId, int userId);

        /// <summary>
        /// Set participant stage to <see cref="ParticipantStage">QUIT</see>
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userId">user id which leaving the event</param>
        Task<ParticipantInfo> ExitSprint(int sprintId, int userId);

        /// <summary>
        /// Get all pariticipant with given stage <see cref="SprintParticipant"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="stage">filter with stage</param>
        /// <returns><see cref="SprintParticipant"> list of participant info</see></returns>
        Task<List<SprintParticipant>> GetParticipants(int sprintId, ParticipantStage stage);

        Task<SprintParticipant> CheckSprintParticipant(int sprintId, int userId);

        /// <summary>
        /// Get all sprints paritipant details with given query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>all pariticpant details</returns>
        IEnumerable<SprintParticipant> GetAll(Expression<Func<SprintParticipant, bool>> query);

        /// <summary>
        /// Filter sprint pariticipant detials with sprint and user details with given query
        /// </summary>
        /// <param name="query">query to filter record</param>
        /// <returns><see cref="SprintParticipant"> entity</see>/<returns>
        Task<SprintParticipant> Get(Expression<Func<SprintParticipant, bool>> query);

        Task<SprintParticipant> AddParticipant(int sprintId, int userId);

        Task<Sprint> GetSprint(int sprintId);
        Task<User> GetParticipant(int userId);
        IQueryable<Notification> GetNotification(int userId);
        Task JoinSprint(int userId, int sprintId);
        Task DeleteParticipant(int userId, int sprintId);
        int GetParticipantCount(int sprintId);
        void RemoveParticipant(SprintParticipant participant);
        IEnumerable<Friend> GetFriends(int userId);
        Task RemoveNotification(int notificationId);
        Task<T> FindWithInclude<T>(Expression<Func<T, bool>> predicate, params string [] includeProperties)where T : class, new();
        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();
    }
}