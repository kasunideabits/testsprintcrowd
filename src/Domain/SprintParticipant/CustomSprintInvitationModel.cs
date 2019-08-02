namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// Custom Sprint invitation model with selected columns.
    /// </summary>
    public class CustomSprintInvitationModel
    {
        /// <summary>
        /// Initialize <see cref="ParticipantInfo">ParticipantInfo class </see>
        /// </summary>
        /// <param name="sprintId">sprint id which participate</param>
        /// <param name="inviterId">id of the inviter</param>
        /// <param name="inviteeId">id of the invitee</param>
        /// <param name="status">invitation status</param>
        public CustomSprintInvitationModel(
            int sprintId,
            int inviterId,
            int inviteeId,
            SprintInvitationStatus status
        )
        {
            this.SprintId = sprintId;
            this.InviterId = inviterId;
            this.InviteeId = inviteeId;
            this.Status = status;
        }

        /// <summary>
        /// Gets or set sprint id for invitiation
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// Gets or set inviter id for the sprint
        /// </summary>
        public int InviterId { get; set; }

        /// <summary>
        /// Gets or set invitee id for the sprint
        /// </summary>
        public int InviteeId { get; set; }

        /// <summary>
        /// Status for sprint invitation
        /// <see cref="SprintInvitationStatus"> status </see>
        /// </summary>
        public SprintInvitationStatus Status { get; set; }

    }
}