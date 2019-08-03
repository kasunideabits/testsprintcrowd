namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;

    public class SprintInvitationService : ISprintInvitationService
    {
        public SprintInvitationService(ISprintInvitationRepo sprintInvitationRepo)
        {
            this.SprintInvitationRepo = sprintInvitationRepo;
        }

        private ISprintInvitationRepo SprintInvitationRepo { get; }

        public async Task Invite(int inviterId, int inviteeId, int sprintId)
        {
            await this.SprintInvitationRepo.Invite(inviterId, inviteeId, sprintId);
            await this.SprintInvitationRepo.AddNotification(inviterId, inviteeId, sprintId);
            this.SprintInvitationRepo.SaveChanges();
            return;
        }
    }
}