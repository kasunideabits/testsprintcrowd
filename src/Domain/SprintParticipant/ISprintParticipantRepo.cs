namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
        /// adds new private event to database
        /// </summary>
        /// <param name="privateEventCreate">event model</param>
        /// <returns>Created sprint details</returns>
        Task<SprintParticipant> AddSprintParticipant(SprintParticipant privateEventCreate);

        /// <summary>
        /// Get current joined users of a given sprint
        /// </summary>
        /// <param name="sprint_type">event model</param>
        /// <param name="sprint_id">event model</param>
        /// <param name="offset">event model</param>
        /// <param name="fetch">event model</param>
        /// <returns>Get current joined users of a given sprint</returns>
        Task<List<CustomSprintModel>> GetCurrentJoinedUsers(int sprint_type, int sprint_id, int offset, int fetch);
        /// Set participant stage to <see cref="ParticipantStage">QUIT</see>
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userId">user id which leaving the event</param>
        Task<ParticipantInfo> ExitSprint(int sprintId, int userId);

        /// <summary>
        /// saves changed to db
        /// </summary>
        void SaveChanges();
    }
}