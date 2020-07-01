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
        /// User mark attendance for an event directly
        /// </summary>
        /// <param name="sprintId">sprint id for join</param>
        /// <param name="userId">user id for who join</param>
        /// <returns>joined user details</returns>
        Task<SprintParticipant> AddSprintParticipantMarkAttendance(int sprintId, int userId);

        /// <summary>
        /// Get all pariticipant with given stage <see cref="SprintParticipant"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <param name="stage">filter with stage</param>
        /// <returns><see cref="SprintParticipant"> list of participant info</see></returns>
        Task<List<SprintParticipant>> GetParticipants(int sprintId, ParticipantStage stage);

        /// <summary>
        /// Get all pariticipant with given sprint <see cref="SprintParticipant"> stage </see>
        /// </summary>
        /// <param name="sprintId">sprint id to lookup</param>
        /// <returns><see cref="SprintParticipant"> list of participant info</see></returns>
        Task<List<SprintParticipant>> GetAllParticipants(int sprintId);

        /// <summary>
        /// Check user exist in sprint
        /// </summary>
        /// <param name="sprintId">sprint id for check</param>
        /// <param name="userId">user id for check</param>
        /// <returns><see cref="SprintParticipant"> participant info</see></returns>
        Task<SprintParticipant> CheckSprintParticipant(int sprintId, int userId);

        /// <summary>
        /// Get all sprints paritipant details with given query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>all pariticpant details</returns>
        IEnumerable<SprintParticipant> GetAll(Expression<Func<SprintParticipant, bool>> query);

        Task<SprintParticipant> GetByUserId(int userId);

        /// <summary>
        /// Filter sprint pariticipant detials with sprint and user details with given query
        /// </summary>
        /// <param name="query">query to filter record</param>
        /// <returns><see cref="SprintParticipant"> entity</see>/<returns>
        Task<SprintParticipant> Get(Expression<Func<SprintParticipant, bool>> query);

        /// <summary>
        /// Add pariticipant to sprint
        /// </summary>
        /// <param name="sprintId">sprint id </param>
        /// <param name="userId">user id</param>
        /// <returns>participant entity</returns>
        Task<SprintParticipant> AddParticipant(int sprintId, int userId);

        /// <summary>
        /// Get sprint details with given sprint id
        /// </summary>
        /// <param name="sprintId">sprint id to fetch</param>
        /// <returns>sprint record</returns>
        Task<Sprint> GetSprint(int sprintId);

        /// <summary>
        /// Get pariticipant with given user id
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>User record</returns>
        Task<User> GetParticipant(int userId);

        /// <summary>
        /// Get notifications for given user id
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>notificaitons</returns>
        IQueryable<NotificationInfo> GetNotification(int userId);

        /// <summary>
        /// Join participant to given sprint
        /// </summary>
        /// <param name="userId">user id who want to participate</param>
        /// <param name="sprintId">sprint id to join</param>
        Task JoinSprint(int userId, int sprintId);

        /// <summary>
        /// Delete pariticipant form sprint
        /// </summary>
        /// <param name="userId">user id for delete</param>
        /// <param name="sprintId">sprint id for delete</param>
        Task DeleteParticipant(int userId, int sprintId);

        // <summary>
        /// Get pariticipant count in given sprint id
        /// </summary>
        /// <param name="sprintId">sprint it to count</param>
        /// <returns>count for participants</returns>
        int GetParticipantCount(int sprintId);

        /// <summary>
        /// Remove particiapnt
        /// </summary>
        /// <param name="participant">participant record</param>
        void RemoveParticipant(SprintParticipant participant);

        /// <summary>
        /// Get friends
        /// </summary>
        /// <param name="userId">user id to check friends</param>
        /// <returns>list of friends</returns>
        IEnumerable<Friend> GetFriends(int userId);

        /// <summary>
        /// Remove notification
        /// </summary>
        /// <param name="notificationId">notificaiton id</param>
        Task RemoveNotification(int notificationId);

        /// <summary>
        /// Remove sprint notifications
        /// </summary>
        /// <param name="sprintId">sprint id to remove</param>
        /// <param name="userId">user id to remove</param>
        void RemoveSprintNotification(int sprintId, int userId);

        /// <summary>
        /// generic method to find with include
        /// </summary>
        /// <typeparam name="T">any database entity</typeparam>
        Task<T> FindWithInclude<T>(Expression<Func<T, bool>> predicate, params string[] includeProperties) where T : class, new();

        /// <summary>
        /// Get joined sprints
        /// </summary>
        /// <param name="userId">user id to fethc</param>
        /// <param name="fetchDate">fetch date</param>
        /// <returns>sprint details</returns>
        IEnumerable<Sprint> GetJoinedSprints(int userId, DateTime fetchDate);

        /// <summary>
        /// Update sprint participant
        /// </summary>
        /// <param name="participant"></param>
        void UpdateParticipant(SprintParticipant participant);

        /// <summary>
        ///  Get dates for user participating for next 7 days
        /// </summary>
        /// <param name="userId">user id </param>
        /// <returns>events available dates</returns>
        List<DateTime> GetNextSevenDaysSprintsDates(int userId);

        /// <summary>
        /// Get sprint creator
        /// </summary>
        /// <param name="sprintId">sprint id</param>
        User GetCreator(int sprintId);

        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();

        IEnumerable<SprintParticipant> GetAllById(int sprintId, Expression<Func<SprintParticipant, bool>> query);
    }
}