namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    /// <summary>
    /// Background notificaiton handler config
    /// </summary>
    public class NotificationWorkerConfig
    {
        /// <summary>
        /// Gets or sets hangfire db connection string
        /// </summary>
        public string HangfireConnection { get; set; }
    }

}