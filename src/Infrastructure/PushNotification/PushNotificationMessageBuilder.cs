namespace SprintCrowd.BackEnd.Infrastructure.PushNotification
{
    using System.Collections.Generic;
    using FirebaseAdmin.Messaging;

    /// <summary>
    /// Push notification message builder
    /// </summary>
    public class PushNotificationMessageBuilder
    {
        /// <summary>
        /// Initialize PushNotificationMessageBuilder class
        /// </summary>
        public PushNotificationMessageBuilder()
        {
            this.FireBaseMessage = new Message();
        }

        private Message FireBaseMessage { get; set; }

        /// <summary>
        /// Data for message
        /// </summary>
        public PushNotificationMessageBuilder Message(Dictionary<string, string> data)
        {
            this.FireBaseMessage.Data = data;
            return this;

        }

        /// <summary>
        /// Token for device
        /// </summary>
        public PushNotificationMessageBuilder Token(string token)
        {
            this.FireBaseMessage.Token = token;
            return this;
        }

        /// <summary>
        /// Add notification title and body
        /// </summary>
        public PushNotificationMessageBuilder Notification(string title, string body)
        {
            var notificaiton = new Notification()
            {
                Title = title,
                Body = body,
            };
            this.FireBaseMessage.Notification = notificaiton;
            return this;
        }

        /// <summary>
        /// Message topic
        /// </summary>
        public PushNotificationMessageBuilder Topic(string topic)
        {
            this.FireBaseMessage.Topic = topic;
            return this;
        }

        /// <summary>
        /// Build  message
        /// </summary>
        public Message Build()
        {
            return this.FireBaseMessage;
        }
    }
}