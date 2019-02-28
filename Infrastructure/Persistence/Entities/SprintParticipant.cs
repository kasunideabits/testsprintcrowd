namespace SprintCrowdBackEnd.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Sprint participant model.
    /// </summary>
    public class SprintParticipant
    {
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>unique id.</value>
        public int Id { get; set; }
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>user who has participated.</value>
        public User User { get; set; }
        /// <summary>
        /// gets or sets value.
        /// </summary>
        /// <value>participant joined or not.</value>
        public int Stage { get; set; }
    }
}