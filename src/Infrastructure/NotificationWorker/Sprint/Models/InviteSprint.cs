namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models
{
    public class InviteSprint
    {
        public InviteSprint(int sprintId, int inviterId, int inviteeId)
        {
            this.SprintId = sprintId;
            this.InviterId = inviterId;
            this.InviteeId = inviteeId;
        }

        public int SprintId { get; }
        public int InviterId { get; }
        public int InviteeId { get; }
    }
}