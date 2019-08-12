namespace SprintCrowd.BackEnd.Domain.Notification.ExitEvent
{
    /// <summary>
    /// Channel name for exited event notifications
    /// </summary>
    public static class ChannelNames
    {
        /// <summary>
        /// Exit event subsribe for sprint manger
        /// </summary>
        /// <param name="sprintId">exit sprint id</param>
        public static string Sprint(int sprintId) => $"sprint{sprintId}";
    }

    /// <summary>
    /// Events name generate for exited evetns notifications
    /// </summary>
    public static class EventNames
    {
        private const string ExitSprint = "Exit";

        /// <summary>
        /// Get exit event sprint manger event
        /// </summary>
        /// <returns>exit event name for sprint manager</returns>
        public static string GetExitSprintEvent() => ExitSprint;
    }
}