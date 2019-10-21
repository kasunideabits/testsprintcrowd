namespace SprintCrowd.BackEnd.Infrastructure.PushNotification
{
    using System.Collections.Generic;
    using FirebaseAdmin.Messaging;

    /// <summary>
    /// Push notification message builder
    /// </summary>
    public class PushNotificationMessageBuilder : IPushNotificationMessageBuilder
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
        public void Message(Dictionary<string, string> data)
        {
            this.FireBaseMessage.Data = data;
        }

        /// <summary>
        /// Token for device
        /// </summary>
        public void Token(string token)
        {
            this.FireBaseMessage.Token = token;
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
        /// Message topic
        /// </summary>
        public void Topic(string topic)
        {
            this.FireBaseMessage.Topic = topic;
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