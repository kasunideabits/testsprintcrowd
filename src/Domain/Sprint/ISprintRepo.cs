namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// inerface for event repo
    /// </summary>
    public interface ISprintRepo
    {

        /// <summary>
        /// Get sprint by given predicate
        /// </summary>
        /// <param name="predicate"> predicate</param>
        Task<Sprint> GetSprint(Expression<Func<Sprint, bool>> predicate);

        /// <summary>
        /// Get all sprints
        /// </summary>
        /// <returns>all sprints</returns>
        Task<List<Sprint>> GetAllEvents();

        /// <summary>
        /// get all sprint public or private
        /// </summary>
        /// <param name="eventType">public or private</param>
        /// <returns>all events with given type</returns>
        Task<List<Sprint>> GetAllEvents(int eventType);

        /// <summary>
        /// Get all events with given date range
        /// </summary>
        /// <param name="from">date range from</param>
        /// <param name="to">date range to</param>
        /// <returns>All sprint with given date range</returns>
        Task<List<Sprint>> GetAllEvents(DateTime from, DateTime to);

        /// <summary>
        /// Get all ongoing sprints
        /// </summary>
        /// <returns>All ongoing sprints</returns>
        Task<List<Sprint>> GetLiveSprints();

        /// <summary>
        /// adds new event to database
        /// </summary>
        /// <param name="eventToCreate">event model</param>
        /// <returns>Created sprint details</returns>
        Task<Sprint> AddSprint(Sprint eventToCreate);

        /// <summary>
        /// adds new event to database
        /// </summary>
        /// <param name="eventToDraft">event model</param>
        /// <returns>Created sprint details</returns>
        Task<Sprint> DraftSprint(Sprint eventToDraft);

        /// <summary>
        /// Updates event in database
        /// </summary>
        /// <param name="sprintData">event model</param>
        Task<Sprint> UpdateSprint(Sprint sprintData);

        /// <summary>
        /// Get the participants with given predicate
        /// sprint id
        /// </summary>
        /// <param name="predicate">predicate for lookup</param>
        /// <returns><see cref="SprintParticipant">sprint pariticipants</see></returns>
        IEnumerable<SprintParticipant> GetParticipants(Expression<Func<SprintParticipant, bool>> predicate);

        /// <summary>
        /// Add paritipant to sprint
        /// </summary>
        /// <param name="userId">user id for pariticipant</param>
        /// <param name="sprintId">sprint id which going to join</param>
        Task AddParticipant(int userId, int sprintId);

        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();
    }
}