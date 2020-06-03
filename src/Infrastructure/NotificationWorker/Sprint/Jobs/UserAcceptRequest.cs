using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowd.BackEnd.Infrastructure.PushNotification;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    /// <summary>
    /// Implement sprint invite notification
    /// </summary>
    public class UserAcceptRequest : IUserAcceptRequest
    {
        private ScrowdDbContext Context { get; }
        private IPushNotificationClient PushNotificationClient { get; }

        public UserAcceptRequest(ScrowdDbContext context, IPushNotificationClient client)
        {
            this.Context = context;
            this.PushNotificationClient = client;
        }

        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"></param>
        public void Run(object message = null)
        {
            {
                AcceptRequest userAccept = message as AcceptRequest;
                if (userAccept != null)
                {
                    this.SendPushNotification(userAccept);
                }
            }
        }


        private void SendPushNotification(AcceptRequest acceptRequest)
        {
            var notificationId = this.AddToDb(acceptRequest); 
            var token = this.GetToken(acceptRequest.RequestSenderId);
            var notification = this.GetNotification(this.UserLanguagePreference(acceptRequest.RequestSenderId));
            var notificationBody = String.Format(notification.Body, acceptRequest.Name);
            var notificationMsg = this.BuildNotificationMessage(notificationId, notification.Title, notificationBody, token, acceptRequest);
            this.PushNotificationClient.SendMulticaseMessage(notificationMsg);

            this.Context.SaveChanges();
        }


        private dynamic BuildNotificationMessage(int notificationId, string title, string body, List<string> tokens, AcceptRequest notificationData)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)SprintNotificaitonType.Edit).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            var message = new PushNotification.PushNotificationMulticastMessageBuilder()
                .Notification(title, body)
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private int AddToDb(AcceptRequest accRequest)
        {
            List<UserNotification> userNotifications = new List<UserNotification>();
            var acceptFriendNotification = new FriendAcceptNoticiation()
            {
                Type = FriendNoticiationType.Accepet,
                RequesterId = accRequest.RequestSenderId,
                Id = accRequest.Id,
                Name = accRequest.Name,
                ProfilePicture = accRequest.ProfilePicture,
                Code = accRequest.Code,
                Email = accRequest.Email,
                City = accRequest.City,
                Country = accRequest.Country,
                CountryCode = accRequest.CountryCode,
                ColorCode = accRequest.ColorCode,
                CreatedDate = accRequest.CreatedDate

            };
            var notification = this.Context.Notification.Add(acceptFriendNotification);
            return notification.Entity.Id;
        }

        private List<string> GetToken(int participantId)
        {
            return this.Context.FirebaseToken
                .Where(f => participantId == f.User.Id)
                .Select(f => f.Token).ToList();
        }

        private SCFireBaseNotificationMessage GetNotification(string userLang)
        {
            JToken translation;
            switch (userLang)
            {
                case LanugagePreference.EnglishUS:
                    translation = JObject.Parse(File.ReadAllText(@"Translation/en.json"));
                    break;
                case LanugagePreference.Swedish:
                    translation = JObject.Parse(File.ReadAllText(@"Translation/se.json"));
                    break;
                default:
                    translation = JObject.Parse(File.ReadAllText(@"Translation/en.json"));
                    break;
            }
            var section = translation["friendAccept"];
            return new SCFireBaseNotificationMessage(section);
        }

        private string  UserLanguagePreference(int senderId)
        {
            return this.Context.User
                .Where(s =>
                    s.Id == senderId).Select(s => s.LanguagePreference).ToString();
        }
    }

}