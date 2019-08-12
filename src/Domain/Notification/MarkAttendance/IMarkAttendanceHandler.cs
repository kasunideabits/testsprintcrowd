namespace SprintCrowd.BackEnd.Domain.Notification.MarkAttendance
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for mark attendence for event
    /// </summary>
    public interface IMarkAttendanceHandler
    {
        /// <summary>
        /// Execute background task for send notifcations and related work
        /// </summary>
        /// <param name="markAttendance"><see cref="MarkAttendance">mark attendance </see></param>
        Task Execute(MarkAttendance markAttendance);
    }
}