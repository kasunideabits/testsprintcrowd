namespace SprintCrowd.BackEnd.Domain.Notification.MarkAttendance
{
    /// <summary>
    /// Helper class for mark attendance event notificaitons
    /// </summary>
    public static class MarkAttendaceHelper
    {
        /// <summary>
        /// Channel name for mark attendace event notifications
        /// </summary>
        public static class Channels
        {
            /// <summary>
            /// Generate channel name with sprint id
            /// </summary>
            /// <param name="sprintId">sprint id for the event</param>
            /// <returns>generated channel name;</returns>
            public static string GetChannel(int sprintId) => $"sprint{sprintId}";
        }

        /// <summary>
        /// Events name generate for mark attendance notifications
        /// </summary>
        public static class Events
        {
            /// <summary>
            /// Get mark attendance event name
            /// </summary>
            /// <returns>mark attendance event name</returns>
            public static string GetEvent() => Event;

            private const string Event = "MarkedAttendece";
        }
    }
}