namespace SprintCrowd.BackEnd.Infrastructure.PushNotification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FirebaseAdmin.Messaging;

    /// <summary>
    /// Push notification clent
    /// </summary>
    public interface IPushNotificationClient
    {
        /// <summary>
        /// Send multitple messages async
        /// </summary>
        Task SendMessageAsync(Message message);

        /// <summary>
        /// Send one message to given token
        /// </summary>
        Task SendAllMessageAsync(List<Message> messages);

        /// <summary>
        /// Send message to multiple devices
        /// </summary>
        Task SendMulticaseMessage(MulticastMessage message);

    }
}