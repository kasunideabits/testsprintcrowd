namespace SprintCrowd.BackEnd.Web.Sprint
{
    /// <summary>
    /// Represent exit event request
    /// </summary>
    public class ExitEventModel
    {
        /// <summary>
        /// Gets or set sprint id for exiting
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// Gets or set user id for exiting
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or set runner distance
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Gets or set race completed duration
        /// </summary>
        public string RaceCompletedDuration { get; set; }

    }
}