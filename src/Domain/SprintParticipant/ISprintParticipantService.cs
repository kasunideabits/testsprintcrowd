namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using System.Collections.Generic;
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
        /// <param name="joinedUserId">sprint information</param>
        /// <returns>update sprint</returns>
        Task<SprintParticipant> CreateSprintJoinee(JoinPrivateSprintModel privateSprintInfo, User joinedUserId);

        /// <summary>
        /// Get joined users for the given sprint
        /// </summary>
        /// <param name="sprint_id">sprint id</param>
        /// <param name="sprint_type">sprint type</param>
        /// <param name="offset">starting position</param>
        /// <param name="fetch">amount to retrieve</param>
        /// <returns>Get joined users for the given sprint</returns>
        Task<List<CustomSprintModel>> GetJoinedUsers(int sprint_type, int sprint_id, int offset, int fetch);

        /// <summary>
        /// Exit sprint which join for event
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        /// <param name="userId">user id which leaving the event</param>
        /// <returns><see cref="ExitSprintResult"> Exist sprint result</see></returns>
        Task<ExitSprintResult> ExitSprint(int sprintId, int userId);

        /// <summary>
        /// Accept or reject sprint invitation
        /// </summary>
        /// <param name="SprintId">Id of the sprint</param>
        /// <param name="InviterId">Id of the inviter</param>
        /// <param name="InviteeId">Id of the invitee</param>
        //Task<SprintInvite> AcceptEvent(int SprintId, int InviterId, int InviteeId);

        /// <summary>
        /// Accept or reject sprint invitation
        /// </summary>
        /// <param name="SprintId">Id of the sprint</param>
        /// <param name="InviterId">Id of the inviter</param>
        /// <param name="InviteeId">Id of the invitee</param>
        //Task<SprintInvite> DeclineEvent(int SprintId, int InviterId, int InviteeId);


    }
}