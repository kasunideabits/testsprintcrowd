using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace SprintCrowd.BackEnd.Infrastructure.PushNotification
{
    /// <summary>
    /// Push notification client who responsible for send fcm push notifications
    /// </summary>
    public class PushNotificationClient : IPushNotificationClient
    {
        /// <summary>
        /// Initialize PushNotificationClient client
        /// </summary>
        public PushNotificationClient(IOptions<FirebaseConfig> config)
        {
            this.CreateFireBaseApp(config.Value.FilePath);
        }

        /// <summary>
        /// Send multitple messages async
        /// </summary>
        public async Task SendAllMessageAsync(List<Message> messages)
        {
            await FirebaseMessaging.DefaultInstance.SendAllAsync(messages);
        }

        /// <summary>
        /// Send one message to given token
        /// </summary>
        public async Task SendMessageAsync(Message message)
        {
            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        /// <summary>
        /// Send message to multiple devices
        /// </summary>
        public async Task SendMulticaseMessage(MulticastMessage message)
        {
            await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
        }

        private void CreateFireBaseApp(string filePath)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(filePath),
                });
            }
        }
    }
}