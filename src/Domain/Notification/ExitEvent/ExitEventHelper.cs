namespace SprintCrowd.BackEnd.Domain.Notification.ExitEvent
{
    /// <summary>
    /// Send notificaitons to all pariticipants x user exited y event
    /// </summary>
    public static class ExitEventHelper
    {
        /// <summary>
        /// Channel name for exited event notifications
        /// </summary>
        public static class Channels
        {
            /// <summary>
            /// Exit event subscribe name
            /// </summary>
            public static string ExitUser() => "notifications:sprint";

            /// <summary>
            /// Exit event subsribe for sprint manger
            /// </summary>
            /// <param name="sprintId">exit sprint id</param>
            public static string ExitSprint(int sprintId) => $"sprint{sprintId}";
        }

        /// <summary>
        /// Events name generate for exited evetns notifications
        /// </summary>
        public static class Events
        {
            /// <summary>
            /// Get exit event name for specific user
            /// </summary>
            /// <param name="userId">user id which want to send notificaiton</param>
            /// <returns>exit event name</returns>
            public static string GetEvent(int userId) => $"{EventUser}{userId}";

            /// <summary>
            /// Get exit event sprint manger event
            /// </summary>
            /// <returns>exit event name for sprint manager</returns>
            public static string GetSprintEvent() => EventSprint;

            private const string EventUser = "exit";
            private const string EventSprint = "Exit";
        }
    }
}