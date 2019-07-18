namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    /// <summary>
    /// Helper class for create channels
    /// </summary>
    public static partial class Channels
    {
        /// <summary>
        /// Generate channel name with sprint id
        /// </summary>
        /// <param name="sprintId">sprint id for the event</param>
        /// <returns>generated channel name;</returns>
        public static string GetChannel(int sprintId) => $"sprint{sprintId}";
    }

    /// <summary>
    /// Events name for publish messages
    /// </summary>
    public static partial class EventName
    {
        /// <summary>
        /// Participant after mark attendance
        /// </summary>
        public const string MarkedAttenence = "MarkedAttendece";
    }
}