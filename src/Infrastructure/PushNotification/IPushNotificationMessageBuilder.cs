namespace SprintCrowd.BackEnd.Infrastructure.PushNotification
{
    using System.Collections.Generic;
    using FirebaseAdmin.Messaging;

    /// <summary>
    /// Interface for push notification message
    /// </summary>
    public interface IPushNotificationMessageBuilder
    {
        /// <summary>
        /// Data for message
        /// </summary>
        void Message(Dictionary<string, string> data);

        /// <summary>
        /// Token for device
        /// </summary>
        void Token(string tokens);

        /// <summary>
        /// Add notification title and body
        /// </summary>
        void Notification(string title, string body);

        /// <summary>
        /// Message topic
        /// </summary>
        void Topic(string topic);

        /// <summary>
        /// Build  message
        /// </summary>
        Message Build();

    }
}