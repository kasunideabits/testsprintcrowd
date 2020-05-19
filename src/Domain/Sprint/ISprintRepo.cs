namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.Sprint.Dlos;
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
        /// Get all sprints with given predicate
        /// </summary>
        /// <param name="predicate">query </param>
        /// <returns>all sprints match to predicate</returns>
        Task<IQueryable<Sprint>> GetSprints(Expression<Func<Sprint, bool>> predicate);

        /// <summary>
        /// Get all sprints
        /// </summary>
        /// <returns>all sprints</returns>
        Task<List<Sprint>> GetAllEvents();

        /// <summary>
        /// get all sprint public or private
        /// </summary>
        /// <param name="eventType">public or private</param>
        /// <param name="searchTerm">Search term to filter</param>
        /// <param name="sortBy">Sort by option</param>
        /// <param name="filterBy">Filter by option</param>
        /// <returns>all events with given type</returns>
        Task<List<Sprint>> GetAllEvents(int eventType, string searchTerm, string sortBy, string filterBy);

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
        /// adds new events to database
        /// </summary>
        /// <param name="eventsToCreate">List of events to be created</param>
        /// <returns>Created sprint details</returns>
        Task AddMultipleSprints(IEnumerable<Sprint> eventsToCreate);

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
        /// <param name="participantStage">sprint participant stage</param>
        Task AddParticipant(int userId, int sprintId, ParticipantStage participantStage = ParticipantStage.PENDING);

        /// <summary>
        /// Remove sprint with given id
        /// </summary>
        /// <param name="sprint">sprint entity</param>
        void RemoveSprint(Sprint sprint);

        /// <summary>
        /// Get friend list for given user id
        /// </summary>
        /// <param name="userId">user id to fetch</param>
        /// <returns>Friends</returns>
        IEnumerable<Friend> GetFriends(int userId);

        Task<User> FindInfluencer(string influencerEmail);

        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();

        Task<UserPreference> GetUserPreference(int userId);

        /// <summary>
        /// Get all sprints by sprint name
        /// </summary>
        /// <param name="sprintName">name of the sprint</param>
        /// <returns>Friends</returns>
        Task<List<String>> GetSprintNames(string sprintName);
    }
}