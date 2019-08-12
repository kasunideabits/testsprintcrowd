namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for sprint invitation service
    /// </summary>
    public interface ISprintInvitationService
    {
        /// <summary>
        /// Invite friend for sprint
        /// </summary>
        /// <param name="inviterId">inviter user id</param>
        /// <param name="inviteeId">invitee user id</param>
        /// <param name="sprintId">sprint id</param>
        Task Invite(int inviterId, int inviteeId, int sprintId);
    }
}