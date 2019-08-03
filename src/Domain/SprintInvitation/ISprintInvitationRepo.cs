namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;

    public interface ISprintInvitationRepo
    {
        Task Invite(int inviterId, int inviteeId, int sprintId);

        Task AddNotification(int senderId, int receiverId, int sprintId);

        /// <summary>
        /// commit and save changes to the db
        /// only call this from the service, DO NOT CALL FROM REPO ITSELF
        /// Unit of work methology.
        /// </summary>
        void SaveChanges();
    }
}