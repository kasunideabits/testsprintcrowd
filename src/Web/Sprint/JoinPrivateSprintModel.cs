namespace SprintCrowd.BackEnd.Web.Event
{
    /// <summary>
    /// model for holding join event data
    /// </summary>
    public class JoinPrivateSprintModel
    {
        /// <summary>
        /// Sprint Id
        /// </summary>
        /// <value>Sprint Id</value>
        public int SprintId { get; set; }

        /// <summary>
        /// User id who joining
        /// </summary>
        /// <value></value>
        public int UserId { get; set; }
    }
}