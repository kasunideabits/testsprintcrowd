namespace SprintCrowd.BackEnd.Web.Sprint
{
    /// <summary>
    ///  Class which reporesent mark attendance
    /// </summary>
    public class MarkAttendence
    {
        /// <summary>
        /// Sprint id
        /// </summary>
        public int SprintId { get; set; }

        /// <summary>
        /// User id who marked attendance
        /// </summary>
        public int UserId { get; set; }
    }
}