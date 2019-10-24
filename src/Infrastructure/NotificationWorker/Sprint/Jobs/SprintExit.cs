namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;

    /// <summary>
    /// Exit event notification handling
    /// </summary>
    public class SprintExit : ISprintExit
    {
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="context">db context</param>
        /// <param name="ablyFactory">ably connection factory</param>
        public SprintExit(ScrowdDbContext context, IAblyConnectionFactory ablyFactory)
        {
            this.Context = context;
            this.AblyConnectionFactory = ablyFactory;
        }

        private ScrowdDbContext Context { get; }
        private IAblyConnectionFactory AblyConnectionFactory { get; }

        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"><see cref="ExitSprint"> exit data </see></param>
        public void Run(object message = null)
        {
            ExitSprint exitSprint = message as ExitSprint;
            if (exitSprint != null)
            {
                this.AblyMessage(exitSprint);
                this.SendPushNotification();
            }
        }

        private void AblyMessage(ExitSprint exitSprint)
        {
            var notificaitonMsg = NotificationMessageMapper(exitSprint);
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + exitSprint.SprintId);
            channel.Publish("Exit", notificaitonMsg);
        }

        private void SendPushNotification() { }

        private static ExitNotification NotificationMessageMapper(ExitSprint exitSprint)
        {
            return new ExitNotification(
                exitSprint.UserId,
                exitSprint.Name,
                exitSprint.ProfilePicture,
                exitSprint.SprintName
            );
        }

    }

    internal class ExitNotification
    {
        /// <summary>
        /// Initialize ExitNotification class
        /// </summary>
        /// <param name="userId">user id for who has exited</param>
        /// <param name="name">name for who has exited</param>
        /// <param name="profilePicture">profile picture url for user who has exited</param>
        /// <param name="sprintName">sprint name</param>
        public ExitNotification(int userId, string name, string profilePicture, string sprintName)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.SprintName = sprintName;
        }

        /// <summary>
        /// Gets users id
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Gets user's name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets url for profile picture
        /// </summary>
        public string ProfilePicture { get; private set; }

        /// <summary>
        /// Gets sprint name
        /// </summary>
        public string SprintName { get; private set; }
    }
}