namespace SprintCrowd.BackEnd.Infrastructure.RealTimeMessage
{
    /// <summary>
    /// Interface for notification factory
    /// </summary>
    public interface IAblyConnectionFactory
    {
        /// <summary>
        /// Create new channel
        /// </summary>
        /// <param name="name">channel name</param>
        /// <returns></returns>
        IChannel CreateChannel(string name);
    }
}