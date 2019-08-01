namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    /// <summary>
    /// Custom Sprint model with only two selected columns.
    /// </summary>
    public class CustomSprintModel
    {
        /// <summary>
        /// gets the sprint.
        /// </summary>
        /// <value></value>
        public int SprintId { get; set; }

        /// <summary>
        /// sets the sprint.
        /// </summary>
        /// <value></value>
        public int UserId { get; set; }

    }


}