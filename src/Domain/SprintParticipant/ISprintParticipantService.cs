namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Web.Event;

    /// <summary>
    /// Interface for sprint participant service
    /// </summary>
    public interface ISprintParticipantService
    {
        /// <summary>
        /// Mark the attendece for the given sprint and notify with evnet
        /// EventName.MarkedAttenence with MarkAttendenceMessage message
        /// </summary>
        /// <param name="sprintId">sprint id for mark attendance</param>
        /// <param name="userId">user id for for participant</param>
        Task MarkAttendence(int sprintId, int userId);

        /// <summary>
        /// update sprint
        /// </summary>
        /// <param name="privateSprintInfo">sprint information</param>
        /// <param name="stage">sprint information</param>
        /// <param name="joinedUserId">sprint information</param>
        /// <returns>update sprint</returns>
        Task<SprintParticipant> CreateSprintJoinee(JoinPrivateSprintModel privateSprintInfo, User joinedUserId);

        /// <summary>
        /// Exit sprint which join for event
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userId">user id which leaving the event</param>
        /// <returns><see cref="ExitSprintResult"> Exist sprint result</see></returns>
        Task<ExitSprintResult> ExitSprint(int sprintId, int userId);
    }
}