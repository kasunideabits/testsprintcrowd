namespace SprintCrowd.BackEnd.Domain.Notification
{
    using System;
    using SprintCrowd.BackEnd.Application;

    /// <summary>
    /// Sprint invitiation realted data
    /// </summary>
    public class SprintInviteInfo
    {
        /// <summary>
        /// Gets or set invitaion id
        /// </summary>
        public int SprintInviteId { get; set; }

        /// <summary>
        /// Gets or set sprint id
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// Gets or set sprint name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Gets or set distance for the sprint
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Gets or set sprint start time
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or set sprint invitation status type
        /// </summary>
        public SprintInvitationStatus Status { get; set; }
    }
}