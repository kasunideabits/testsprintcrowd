namespace SprintCrowd.BackEnd.Infrastructure.Notifier
{
    /// <summary>
    /// Interface for notification factory
    /// </summary>
    public interface INotifyFactory
    {
        /// <summary>
        /// Create new channel
        /// </summary>
        /// <param name="name">channel name</param>
        /// <returns></returns>
        IChannel CreateChannel(string name);
    }
}