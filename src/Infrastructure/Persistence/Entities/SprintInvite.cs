namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System.Collections.Generic;
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// Entity which describe sprint invite table
    /// </summary>
    public class SprintInvite
    {
        /// <summary>
        /// Gets or set unique id for sprint invitation
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Gets or set inviter reference
        /// </summary>
        public virtual User Inviter { get; set; }

        /// <summary>
        ///  Gets or set invitee reference
        /// </summary>
        public virtual User Invitee { get; set; }

        /// <summary>
        /// Referance for sprint
        /// </summary>
        public virtual Sprint Sprint { get; set; }

        /// <summary>
        /// Add refenrece for notificaton
        /// </summary>
        public virtual List<Notification> Notification { get; set; }
    }
}