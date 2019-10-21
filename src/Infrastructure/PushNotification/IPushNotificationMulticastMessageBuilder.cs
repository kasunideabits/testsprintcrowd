namespace SprintCrowd.BackEnd.Infrastructure.PushNotification
{
    using System.Collections.Generic;
    using FirebaseAdmin.Messaging;

    /// <summary>
    /// Interface for multicast message
    /// </summary>
    public interface IPushNotificationMulticastMessageBuilder
    {
        /// <summary>
        /// Data for message
        /// </summary>
        void Message(Dictionary<string, string> data);

        /// <summary>
        /// Tokens for devices
        /// </summary>
        void Tokens(List<string> tokens);

        /// <summary>
        /// Add notification title and body
        /// </summary>
        void Notification(string title, string body);

        /// <summary>
        /// Build multicast
        MulticastMessage Build();
    }
}