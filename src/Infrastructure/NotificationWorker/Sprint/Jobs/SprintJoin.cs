namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;

    /// <summary>
    /// join sprint notification handling
    /// </summary>
    public class SprintJoin : ISprintJoin
    {
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="context">db context</param>
        /// <param name="ablyFactory">ably connection factory</param>
        public SprintJoin(ScrowdDbContext context, IAblyConnectionFactory ablyFactory)
        {
            this.Context = context;
            this.AblyConnectionFactory = ablyFactory;
        }

        private ScrowdDbContext Context { get; }
        private IAblyConnectionFactory AblyConnectionFactory { get; }

        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"><see cref="JoinSprint"> join data </see></param>
        public void Run(object message = null)
        {
            JoinSprint joinSprint = message as JoinSprint;
            if (joinSprint != null)
            {
                this.AblyMessage(joinSprint);
                this.SendPushNotification();
            }
        }

        private void AblyMessage(JoinSprint joinSprint)
        {
            System.Console.WriteLine(joinSprint.Name);
        }

        private void SendPushNotification() { }
    }
}