namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public interface ISprintInvitationRepo
    {
        /// <summary>
        /// Add sprint invitaiton
        /// </summary>
        /// <param name="inviterId">inviter user id</param>
        /// <param name="inviteeId">invite user id</param>
        /// <param name="sprintId">sprint id</param>
        /// <returns> sprint invite id </returns>
        Task<int> Invite(int inviterId, int inviteeId, int sprintId);

        /// <summary>
        /// Add sprint notification to notifcation table
        /// </summary>
        /// <param name="senderId">Sender user id</param>
        /// <param name="receiverId">Receiver user id</param>
        /// <param name="sprintInviteId">Sprint invite id</param>
        // Task AddNotification(int senderId, int receiverId, int sprintInviteId);

        Task<SprintInvite> Get(int inviterId, int inviteeId, int sprintId);

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        Task SaveChanges();
    }
}