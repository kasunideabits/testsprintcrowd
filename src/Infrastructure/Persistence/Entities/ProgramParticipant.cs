namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Sprint participant model.
    /// </summary>
    public class ProgramParticipant : BaseEntity
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
        /// Gets or set program
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>participant joined or not.</value>
        public ProgramParticipantStage Stage { get; set; }

        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>user who has participated.</value>
        public virtual User User { get; set; }

        /// <summary>
        /// gets or sets the SprintProgram.
        /// </summary>
        public virtual SprintProgram SprintProgram { get; set; }

       
    }
}