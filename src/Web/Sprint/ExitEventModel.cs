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
    }
}