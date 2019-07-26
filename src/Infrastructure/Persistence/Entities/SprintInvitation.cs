namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Class define SprintInvitation table attributes
    /// </summary>
    public class SprintInvitation
    {
        /// <summary>
        /// Gets or set unique id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// sprint id
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// Inviter id
        /// </summary>
        public int InviterId { get; set; }

        /// <summary>
        /// Invitee id
        /// </summary>
        public int InviteeId { get; set; }

        /// <summary>
        /// sprint reference
        /// </summary>
        public virtual Sprint Sprint { get; set; }

        /// <summary>
        ///  Inviter user reference
        /// </summary>
        public virtual User Inviter { get; set; }

        /// <summary>
        /// Invitee user reference
        /// </summary>
        public virtual User Invitee { get; set; }
    }
}