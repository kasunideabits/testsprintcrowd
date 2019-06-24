namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    /// <summary>
    /// inerface for event repo
    /// </summary>
    public interface ISprintRepo
    {
        /// <summary>
        /// get all sprint public or private
        /// </summary>
        /// <param name="eventType">public or private</param>
        /// <returns>all events with given type</returns>
        Task<List<Sprint>> GetAllEvents(int eventType);

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
        /// Updates event in database
        /// </summary>
        /// <param name="sprintData">event model</param>
        Task<Sprint> UpdateSprint(Sprint sprintData);

        /// <summary>
        /// Check event in database
        /// </summary>
        /// <param name="sprintID">event model</param>
        Task<Sprint> GetSprint(int sprintID);

        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();
    }
}