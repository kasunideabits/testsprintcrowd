namespace SprintCrowd.BackEnd.Web.Event
{
    /// <summary>
    /// model for holding join event data
    /// </summary>
    public class PrivateSprintModel
    {
        /// <summary>
        /// Sprint Id
        /// </summary>
        /// <value>Sprint Id</value>
        public int SprintId { get; set; }

        /// <summary>
        /// IsConfirmed
        /// </summary>
        /// <value>if confirmed</value>
        public bool IsConfirmed { get; set; }
    }
}