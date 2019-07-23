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