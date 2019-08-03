namespace SprintCrowd.BackEnd.Domain.SprintInvitation
{
    using System.Threading.Tasks;

    public interface ISprintInvitationService
    {
        Task Invite(int inviterId, int inviteeId, int sprintId);
    }
}