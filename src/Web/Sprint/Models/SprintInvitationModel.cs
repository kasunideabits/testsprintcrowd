namespace SprintCrowd.BackEnd.Web.Sprint.Models
{
    /// <summary>
    /// Invite sprint request model
    /// </summary>
    public class SprintInvitationModel
    {
        /// <summary>
        /// Gets or set inviter id
        /// </summary>
        public int InviterId { get; set; }

        /// <summary>
        /// Gets or set invitee id
        /// </summary>
        /// <value></value>
        public int InviteeId { get; set; }

        /// <summary>
        /// Gets or set sprint id
        /// </summary>
        public int SprintId { get; set; }
    }
}