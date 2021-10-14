namespace SprintCrowd.BackEnd.Web.Event
{
    /// <summary>
    /// model for holding Sprint Participant Members  data
    /// </summary>
    public class SprintParticipantMembersModel
    {
        /// <summary>
        /// Gets or set Sprint Id
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// Member Id
        /// </summary>
        public string MemberId { get; set; }
    }
}