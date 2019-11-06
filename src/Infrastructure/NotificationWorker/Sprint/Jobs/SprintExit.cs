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
                this.SendPushNotification(exitSprint);
            }
        }

        private void AblyMessage(ExitSprint exitSprint)
        {
            var notificaitonMsg = NotificationMessageMapper.AblyNotificationMessageMapper(exitSprint);
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + exitSprint.SprintId);
            channel.Publish("Exit", notificaitonMsg);
        }

        private void SendPushNotification(ExitSprint exitSprint)
        {
            if (exitSprint.SprintType == Application.SprintType.PrivateSprint)
            {
                // do realy need to send push notification ?
            }
        }

    }

    internal class ExitNotification
    {
        /// <summary>
        /// Initialize ExitNotification class
        /// </summary>
        public ExitNotification(int userId, string name, string profilePicture, string code, string city, string country, string countryCode, string sprintName)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture ?? string.Empty;
            this.Code = code ?? string.Empty;
            this.City = city ?? string.Empty;
            this.Country = country ?? string.Empty;
            this.CountryCode = countryCode ?? string.Empty;
            this.SprintName = sprintName;
        }

        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string ProfilePicture { get; private set; }
        public string Code { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public string CountryCode { get; private set; }

        public string SprintName { get; private set; }
    }

    internal static class NotificationMessageMapper
    {
        public static ExitNotification AblyNotificationMessageMapper(ExitSprint exitSprint)
        {
            return new ExitNotification(
                exitSprint.UserId,
                exitSprint.Name,
                exitSprint.ProfilePicture,
                exitSprint.Code,
                exitSprint.City,
                exitSprint.Country,
                exitSprint.CountryCode,
                exitSprint.SprintName
            );
        }
    }
}