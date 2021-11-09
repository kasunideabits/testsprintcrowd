using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Domain.SprintParticipant;
using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowd.BackEnd.Infrastructure.PushNotification;
using SprintCrowdBackEnd.Infrastructure.NotificationWorker.Sprint.Models;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    public class UserInviteRequest : IUserInviteRequest
    {
        private ScrowdDbContext Context { get; }
        private IPushNotificationClient PushNotificationClient { get; }

        public UserInviteRequest(ScrowdDbContext context, IPushNotificationClient client, ISprintParticipantRepo sprintParticipantRepo)
        {
            this.Context = context;
            this.PushNotificationClient = client;
            this.SprintParticipantRepo = sprintParticipantRepo;
        }

        private ISprintParticipantRepo SprintParticipantRepo { get; }
        private int ParticipantUserId { get; set; }
        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"></param>
        public void Run(object message = null)
        {
            {
                InviteFriend userInvite = message as InviteFriend;
                if (userInvite != null)
                {
                    this.SendPushNotification(userInvite);
                }
            }
        }


        private void SendPushNotification(InviteFriend acceptRequest)
        {
            var notificationId = this.AddToDb(acceptRequest);
            var token = this.GetToken(acceptRequest.RequestSenderId);
            var notification = this.GetNotification(this.UserLanguagePreference(acceptRequest.RequestSenderId));
           // this.ParticipantUserId = acceptRequest.RequestSenderId;
            var notificationBody = String.Format(notification.Body, acceptRequest.UserName);
            var notificationMsg = this.BuildNotificationMessage(notificationId, notification.Title, notificationBody, token, acceptRequest);
            this.PushNotificationClient.SendMulticaseMessage(notificationMsg);

            this.Context.SaveChanges();
        }


        private dynamic BuildNotificationMessage(int notificationId, string title, string body, List<string> tokens, InviteFriend notificationData)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "FriendType");
            
            data.Add("SubType", notificationData.IsCommunity == true ? ((int)SprintNotificaitonType.CommunityInvitationRequest).ToString() : ((int)SprintNotificaitonType.InvitationRequest).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));

            //int badge = this.SprintParticipantRepo != null ? this.SprintParticipantRepo.GetParticipantUnreadNotificationCount(this.ParticipantUserId) : 0;
            //data.Add("Count", badge.ToString());

            var message = new PushNotification.PushNotificationMulticastMessageBuilder(this.SprintParticipantRepo, this.ParticipantUserId)
                .Notification(title, body)
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private int AddToDb(InviteFriend accRequest)
        {
            var sprintNotification = new SprintNotification()
            {
                SprintNotificationType = SprintNotificaitonType.InvitationRequest,
                UpdatorId = accRequest.RequestSenderId,
                StartDateTime = DateTime.UtcNow,
                SprintId = 0,
                SprintName = string.Empty,
                Distance = 0,

            };
            var notification = this.Context.Notification.Add(sprintNotification);
            this.Context.SaveChanges();
            var userNotification = new UserNotification
            {
                SenderId = accRequest.UserId,
                ReceiverId = accRequest.RequestSenderId,
                NotificationId = notification.Entity.Id,
                BadgeValue = 1,
                IsCommunity = accRequest.IsCommunity,
            };
            this.Context.UserNotification.Add(userNotification);
            this.Context.SaveChanges();
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

        private string UserLanguagePreference(int senderId)
        {
            return this.Context.User
                .Where(s =>
                    s.Id == senderId).Select(s => s.LanguagePreference).ToString();
        }
    }
}
