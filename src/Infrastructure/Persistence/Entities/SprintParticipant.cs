namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;
    using SprintCrowd.BackEnd.Application;

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
        /// Mark attendance time which user participate to the sprint
        /// </summary>
        public DateTime StartedTime { get; set; }

        /// <summary>
        /// Finish or left time
        /// </summary>
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// Distance ran till leave or finish the sprint
        /// </summary>
        public int DistanceRan { get; set; }

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