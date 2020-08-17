namespace SprintCrowd.BackEnd.Infrastructure.RealTimeMessage
{
    using System;
    using System.Threading.Tasks;
    using IO.Ably;
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

        /// <summary>
        /// Channel for publish
        /// </summary>
        public IRealtimeChannel Channel { get; }

        /// <summary>
        /// Publish message to given event name
        /// </summary>
        /// <param name="eventName">event name for publish</param>
        /// <param name="message">message to publish</param>
        public async Task Publish(string eventName, dynamic message)
        {
            try
            {
                
                Console.WriteLine("Channel status when Publish:" + this.Channel.State);
                Console.WriteLine("Channel Name when Publish:" + this.Channel.Name);
                if (this.Channel.State == ChannelState.Detached)
                {
                    Console.WriteLine("Channel Detached Name  when Publish:" + this.Channel.Name);
                    Result result = await this.Channel.AttachAsync();
                    if (result.IsFailure)
                    {
                        Console.WriteLine("Attach failed  when Publish: " + result.Error.Message);
                    }
                    else if (this.Channel.State == ChannelState.Attached)
                    {
                        Result resultSucess = await this.Channel.PublishAsync(eventName, message); 
                        if (resultSucess.IsFailure)
                        {
                            Console.WriteLine("Unable to publish message at Channel attach. Reason: " + resultSucess.Error.Message);
                        }
                    }
                }
                else if (this.Channel.State == ChannelState.Attached)
                {
                    Result result = await this.Channel.PublishAsync(eventName, message);
                    if (result.IsFailure)
                    {
                        Console.WriteLine("Unable to publish message. Reason: " + result.Error.Message);
                    }
                }
                else if (this.Channel.State == ChannelState.Failed)
                {
                    Console.WriteLine("ChannelState Failed  when Publish. Reason: " + this.Channel.ErrorReason);
                }
            }
            catch (System.Exception e)
            {
                throw new Application.ApplicationException($"Ably channel error {e}");
            }
        }
    }
}