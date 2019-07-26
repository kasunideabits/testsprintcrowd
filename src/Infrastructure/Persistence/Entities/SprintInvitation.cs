namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    public class SprintInvitation
    {
        public int Id { get; set; }

        public int SprintId { get; set; }

        public int InviterId { get; set; }

        public int InviteeId { get; set; }

        public virtual Sprint Sprint { get; set; }

        public virtual User Inviter { get; set; }

        public virtual User Invitee { get; set; }

    }
}