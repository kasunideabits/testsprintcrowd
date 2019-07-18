namespace SprintCrowd.BackEnd.Web.Event
{
    /// <summary>
    /// model for holding join event data
    /// </summary>
    public class PrivateSprintModel
    {
        /// <summary>
        /// constructor for join event data
        /// </summary>
        // public PrivateSprintModel(int sprintid, bool isconfirmed)
        // {
        //     this.SprintId = sprintid;
        //     this.IsConfirmed = isconfirmed;
        // }
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