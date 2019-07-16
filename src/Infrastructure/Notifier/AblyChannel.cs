namespace SprintCrowd.BackEnd.Infrastructure.Notifier
{
    using System.Threading.Tasks;
    using IO.Ably.Realtime;

    /// <summary>
    /// Implements IChannel interface
    /// </summary>
    public class AblyChannel : IChannel
    {
        /// <summary>
        /// Initialize AblyChannel calss
        /// </summary>
        /// <param name="channel">channel instance</param>
        public AblyChannel(IRealtimeChannel channel)
        {
            this.Channel = channel;
        }

        public IRealtimeChannel Channel { get; }

        /// <summary>
        /// Publish message to given event name
        /// </summary>
        /// <param name="eventName">event name for publish</param>
        /// <param name="message">message to publish</param>
        public async Task Publish(string eventName, dynamic message)
        {
            this.Channel.PublishAsync(eventName, message);
        }
    }
}