namespace SprintCrowd.BackEnd.Infrastructure.Notifier
{
    using IO.Ably;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Implements INotifyFactory interface
    /// </summary>
    public class NotifyFactory : INotifyFactory
    {
        /// <summary>
        /// Initialize NotifyFactory class
        /// </summary>
        public NotifyFactory()
        {
            this.Connection = new AblyRealtime(GetApiKey());

        }

        /// <summary>
        /// Gets ably connection
        /// </summary>
        /// <value>ably connection instance</value>
        private AblyRealtime Connection { get; }

        /// <summary>
        /// Create new ably channel
        /// </summary>
        /// <param name="name">channel name</param>
        /// <returns>IChannel instance</returns>
        public IChannel CreateChannel(string name)
        {
            return new AblyChannel(this.Connection.Channels.Get(name));
        }

        /// <summary>
        /// Get ably API key
        /// </summary>
        /// <returns>ably API key</returns>
        private static string GetApiKey()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return configuration.GetValue<string>("Ably:ApiKey");
        }
    }
}