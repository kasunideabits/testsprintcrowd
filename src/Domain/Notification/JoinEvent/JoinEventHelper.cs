namespace SprintCrowd.BackEnd.Domain.Notification.JoinEvent
{
    /// <summary>
    /// Helper class for joined event notificaitons
    /// </summary>
    public static class JoinEventHelper
    {
        /// <summary>
        /// Channel name for joined event notifications
        /// </summary>
        public static class Channels
        {
            /// <summary>
            /// Join event subscribe name
            /// </summary>
            public const string Join = "notifications:sprint";

            /// <summary>
            /// Generate channel name with sprint id
            /// </summary>
            /// <param name="sprintId">sprint id for the event</param>
            /// <returns>generated channel name;</returns>
            public static string GetSprintChannel(int sprintId) => $"sprint{sprintId}";
        }

        /// <summary>
        /// Events name generate for joined evetns notifications
        /// </summary>
        public static class Events
        {
            /// <summary>
            /// Get joined event name for spefici user
            /// </summary>
            /// <param name="userId">user id which want to send notificaiton</param>
            /// <returns>joined event name</returns>
            public static string GetEvent(int userId) => $"{Event}{userId}";

            private const string Event = "joined";
        }
    }
}