namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;

    public class SprintParticipantRemove : ISprintParticipantRemove
    {

        public SprintParticipantRemove(ScrowdDbContext context, IAblyConnectionFactory ablyConnectionFactory, IPushNotificationClient pushNotificationClient)
        {
            this.Context = context;
            this.AblyConnectionFactory = ablyConnectionFactory;
            this.PushNotificationClient = pushNotificationClient;
        }

        private ScrowdDbContext Context { get; }
        private IAblyConnectionFactory AblyConnectionFactory { get; }
        private IPushNotificationClient PushNotificationClient { get; }


        public void Run(Object message = null)
        {

            RemoveParticipant removeParticipant = message as RemoveParticipant;
            if (removeParticipant != null)
            {
                this.AblyMessage(removeParticipant);
                this.SendPushNotification(removeParticipant);
            }
        }

        private void AblyMessage(RemoveParticipant removeParticipant)
        {
            var notificaitonMsg = RemoveParticipantNotificationMessageMapper.AblyNotificationMessageMapper(removeParticipant);
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + removeParticipant.SprintId);
            channel.Publish("ParticipantRemove", notificaitonMsg);
        }

        private void SendPushNotification(RemoveParticipant removeParticipant)
        {
            if (removeParticipant.SprintType == Application.SprintType.PrivateSprint)
            {
                // do realy need to send push notification ?
                int notificationId = this.AddToDb(removeParticipant, removeParticipant.CreatorId);
                var notificationData = RemoveParticipantNotificationMessageMapper.PushNotificationMessgeMapper(removeParticipant);
                var user = this.GetUser(removeParticipant.UserId);
                var tokens = this.GetTokens(removeParticipant.UserId);
                var notification = this.GetNotification(user.LanguagePreference);
                var notificationBody = String.Format(notification.Body, removeParticipant.CreatorName, removeParticipant.SprintName);
                var notificationMessage = this.BuildNotificationMessage(notificationId, notification.Title, notificationBody, tokens, notificationData);
                this.PushNotificationClient.SendMulticaseMessage(notificationMessage);
                this.Context.SaveChanges();
            }
        }

        private dynamic BuildNotificationMessage(int notificationId, string notificationTitle, string notificationBody, List<string> tokens, ParticipantRemovePushNotificationMesssage notificationData)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)SprintNotificaitonType.RemoveParticipsnt).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            var message = new PushNotification.PushNotificationMulticastMessageBuilder()
                .Notification(notificationTitle, notificationBody)
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private int AddToDb(RemoveParticipant removeParticipant, int creatorId)
        {
            var sprintNotification = new SprintNotification()
            {
                SprintNotificationType = SprintNotificaitonType.RemoveParticipsnt,
                UpdatorId = removeParticipant.CreatorId,
                SprintId = removeParticipant.SprintId,
                SprintName = removeParticipant.SprintName,
                Distance = removeParticipant.Distance,
                StartDateTime = removeParticipant.StartTime,
                SprintType = removeParticipant.SprintType,
                SprintStatus = removeParticipant.SprintStatus,
                NumberOfParticipants = removeParticipant.NumberOfParticipant,
            };
            var notification = this.Context.Notification.Add(sprintNotification);
            var userNotification = new UserNotification
            {
                SenderId = creatorId,
                ReceiverId = removeParticipant.UserId,
                NotificationId = notification.Entity.Id,
            };
            this.Context.UserNotification.Add(userNotification);
            return notification.Entity.Id;
        }

        private List<string> GetTokens(int creatorId)
        {
            return this.Context.FirebaseToken
                .Where(f => f.User.Id == creatorId && f.User.UserState == UserState.Active)
                .Select(f => f.Token).ToList();
        }

        private User GetUser(int userId)
        {
            return this.Context.User.FirstOrDefault(u => u.Id == userId && u.UserState == UserState.Active);
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
            var section = translation["sprintParticipantRemove"];
            return new SCFireBaseNotificationMessage(section);
        }

    }

    internal class ParticipantRemoveNotification
    {
        /// <summary>
        /// Initialize ExitNotification class
        /// </summary>
        public ParticipantRemoveNotification(int sprintId, int userId, string name, string profilePicture, string code, string city, string country, string countryCode, string sprintName)
        {
            this.SprintId = sprintId;
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture ?? string.Empty;
            this.Code = code ?? string.Empty;
            this.City = city ?? string.Empty;
            this.Country = country ?? string.Empty;
            this.CountryCode = countryCode ?? string.Empty;
            this.SprintName = sprintName;
        }

        public int SprintId { get; private set; }
        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string ProfilePicture { get; private set; }
        public string Code { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public string CountryCode { get; private set; }

        public string SprintName { get; private set; }
    }

    internal class ParticipantRemovePushNotificationMesssage
    {
        public ParticipantRemovePushNotificationMesssage(int sprintId, string sprintName, int distance, DateTime startTime, int numberOfParticipants, SprintType type, SprintStatus status,
            int id, string name, string profilePicture, string code, string city, string country, string countryCode)
        {
            this.Sprint = new SprintInfo(sprintId, sprintName, distance, startTime, numberOfParticipants, type, status);
            this.DeletedBy = new UserInfo(id, name, profilePicture, code, city, country, countryCode);
        }

        public SprintInfo Sprint { get; }
        public UserInfo DeletedBy { get; }

        internal sealed class SprintInfo
        {
            public SprintInfo(int id, string name, int distance, DateTime startTime, int numberOfParticipants, SprintType type, SprintStatus status)
            {
                this.Id = id;
                this.Name = name;
                this.Distance = distance;
                this.StartTime = startTime;
                this.NumberOfParticipants = numberOfParticipants;
                this.SprintStatus = status;
                this.SprintType = type;
            }

            public int Id { get; }
            public string Name { get; }
            public int Distance { get; }
            public DateTime StartTime { get; }
            public int NumberOfParticipants { get; }
            public SprintStatus SprintStatus { get; }
            public SprintType SprintType { get; }
        }

        internal sealed class UserInfo
        {
            public UserInfo(int id, string name, string profilePicture, string code, string city, string country, string countryCode)
            {
                this.Id = id;
                this.Name = name;
                this.ProfilePicture = profilePicture ?? string.Empty;
                this.Code = code ?? string.Empty;
                this.City = city ?? string.Empty;
                this.Country = country ?? string.Empty;
                this.CountryCode = countryCode ?? string.Empty;
            }

            public int Id { get; }
            public string Name { get; }
            public string ProfilePicture { get; }
            public string Code { get; }
            public string ColorCode { get; }
            public string City { get; }
            public string Country { get; }
            public string CountryCode { get; }
        }
    }

    internal static class RemoveParticipantNotificationMessageMapper
    {
        public static ParticipantRemoveNotification AblyNotificationMessageMapper(RemoveParticipant exitSprint)
        {
            return new ParticipantRemoveNotification(
                exitSprint.SprintId,
                exitSprint.UserId,
                exitSprint.Name,
                exitSprint.ProfilePicture,
                exitSprint.Code,
                exitSprint.City,
                exitSprint.Country,
                exitSprint.CountryCode,
                exitSprint.SprintName
            );
        }

        public static ParticipantRemovePushNotificationMesssage PushNotificationMessgeMapper(RemoveParticipant exitSprint)
        {
            return new ParticipantRemovePushNotificationMesssage(
                exitSprint.SprintId,
                exitSprint.SprintName,
                exitSprint.Distance,
                exitSprint.StartTime,
                exitSprint.NumberOfParticipant,
                exitSprint.SprintType,
                exitSprint.SprintStatus,
                exitSprint.UserId,
                exitSprint.Name,
                exitSprint.ProfilePicture,
                exitSprint.Code,
                exitSprint.City,
                exitSprint.Country,
                exitSprint.CountryCode
            );
        }
    }
}