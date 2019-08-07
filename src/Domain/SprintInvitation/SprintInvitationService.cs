namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;

    /// <summary>
    /// Handle sprint invitations
    /// </summary>
    public class SprintInvitationService : ISprintInvitationService
    {
        /// <summary>
        /// Intialize <see cref="SprintInvitationService"> class</see>
        /// </summary>
        /// <param name="sprintInvitationRepo">sprint invitation repository</param>
        public SprintInvitationService(ISprintInvitationRepo sprintInvitationRepo)
        {
            this.SprintInvitationRepo = sprintInvitationRepo;
        }

        private ISprintInvitationRepo SprintInvitationRepo { get; }

        /// <summary>
        /// Invite friend for sprint
        /// </summary>
        /// <param name="inviterId">inviter user id</param>
        /// <param name="inviteeId">invitee user id</param>
        /// <param name="sprintId">sprint id</param>
        public async Task Invite(int inviterId, int inviteeId, int sprintId)
        {
            var alreadyExist = await this.SprintInvitationRepo.Get(inviterId, inviteeId, sprintId);
            if (alreadyExist == null)
            {
                var sprintInviteId = await this.SprintInvitationRepo.Invite(inviterId, inviteeId, sprintId);
                await this.SprintInvitationRepo.AddNotification(inviterId, inviteeId, sprintInviteId);
                await this.SprintInvitationRepo.SaveChanges();
                return;
            }
            else
            {
                throw new Application.ApplicationException((int)SprintInviteErrorCode.AlreadySentInvitation, "Already sent invitation");
            }

        }
    }
}