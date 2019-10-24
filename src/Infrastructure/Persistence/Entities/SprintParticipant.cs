using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Sprint participant model.
    /// </summary>
    public class SprintParticipant : BaseEntity
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id.</value>
        public int Id { get; set; }

        /// <summary>
        ///  Gets or set id for participant
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or set sprint
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>participant joined or not.</value>
        public ParticipantStage Stage { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>user who has participated.</value>
        public virtual User User { get; set; }
        /// <summary>
        /// gets or sets the sprint.
        /// </summary>
        public virtual Sprint Sprint { get; set; }

    }
}