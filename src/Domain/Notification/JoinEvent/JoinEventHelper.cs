namespace SprintCrowd.BackEnd.Domain.Notification.JoinEvent
{
    /// <summary>
    /// Channel name for joined event notifications
    /// </summary>
    public static class ChannelNames
    {
        /// <summary>
        /// Join event subscribe name
        /// </summary>
        public const string Join = "notifications:sprint";
    }

    /// <summary>
    /// Events name generate for joined evetns notifications
    /// </summary>
    public static class EventNames
    {
        private const string Event = "joined";

        /// <summary>
        /// Get joined event name for spefici user
        /// </summary>
        /// <param name="userId">user id which want to send notificaiton</param>
        /// <returns>joined event name</returns>
        public static string GetEvent(int userId) => $"{Event}{userId}";
    }
}