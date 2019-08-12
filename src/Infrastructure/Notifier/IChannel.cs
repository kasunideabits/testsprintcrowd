namespace SprintCrowd.BackEnd.Infrastructure.Notifier
{
    using System.Threading.Tasks;

    /// <summary>
    /// Notifi channel implementation
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Publish message
        /// </summary>
        /// <param name="eventName">event name to publsh</param>
        /// <param name="message">message to publish</param>
        /// <returns></returns>
        Task Publish(string eventName, dynamic message);
    }
}