using System.Collections.Generic;
using FirebaseAdmin.Messaging;

namespace SprintCrowd.BackEnd.Infrastructure.PushNotification
{
    // <summary>
    /// Muticast message builder for send notification for multiple devices
    /// </summary>/
    public class PushNotificationMulticastMessageBuilder : IPushNotificationMulticastMessageBuilder
    {
        /// <summary>
        /// Initialize PushNotificationMulticastMessageBuilder class
        /// </summary>
        public PushNotificationMulticastMessageBuilder()
        {
            this.FireBaseMessage = new MulticastMessage();
        }

        private MulticastMessage FireBaseMessage { get; set; }

        /// <summary>
        /// Data for message
        /// </summary>
        public void Message(Dictionary<string, string> data)
        {
            this.FireBaseMessage.Data = data;
        }

        /// <summary>
        /// Tokens for devices
        /// </summary>
        public void Tokens(List<string> tokens)
        {
            this.FireBaseMessage.Tokens = tokens;
        }

        /// <summary>
        /// Add notification title and body
        /// </summary>
        public void Notification(string title, string body)
        {
            var notificaiton = new Notification()
            {
                Title = title,
                Body = body,
            };
            this.FireBaseMessage.Notification = notificaiton;
        }

        /// <summary>
        /// Build multicast message
        /// </summary>
        public MulticastMessage Build()
        {
            return this.FireBaseMessage;
        }
    }
}