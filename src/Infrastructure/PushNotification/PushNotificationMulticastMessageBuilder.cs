using System.Collections.Generic;
using FirebaseAdmin.Messaging;
using SprintCrowd.BackEnd.Domain.SprintParticipant;

namespace SprintCrowd.BackEnd.Infrastructure.PushNotification
{
    // <summary>
    /// Muticast message builder for send notification for multiple devices
    /// </summary>/
    public class PushNotificationMulticastMessageBuilder
    {
        /// <summary>
        /// Initialize PushNotificationMulticastMessageBuilder class
        /// </summary>
        public PushNotificationMulticastMessageBuilder(ISprintParticipantRepo sprintParticipantRepo, int participantUserId)
        {
            
            this.FireBaseMessage = new MulticastMessage();
            this.FireBaseMessage.Apns = new ApnsConfig()
            {
                Aps = new Aps()
                {
                    Badge = sprintParticipantRepo != null ? sprintParticipantRepo.GetParticipantUnreadNotificationCount(participantUserId) :0,
                    CriticalSound = new CriticalSound()
                    {
                        Critical = false,
                        Name = "default",
                        Volume = 1.0
                    }
                }
            };
        }

       
        private MulticastMessage FireBaseMessage { get; set; }
        private  int ParticipantUser { get; set; }

        /// <summary>
        /// Data for message
        /// </summary>
        public PushNotificationMulticastMessageBuilder Message(Dictionary<string, string> data)
        {
            this.FireBaseMessage.Data = data;
            return this;
        }

        /// <summary>
        /// Tokens for devices
        /// </summary>
        public PushNotificationMulticastMessageBuilder Tokens(List<string> tokens)
        {
            this.FireBaseMessage.Tokens = tokens;
            return this;
        }

        /// <summary>
        /// Add notification title and body
        /// </summary>
        public PushNotificationMulticastMessageBuilder Notification(string title, string body)
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
        /// Build multicast message
        /// </summary>
        public MulticastMessage Build()
        {
            return this.FireBaseMessage;
        }
    }
}