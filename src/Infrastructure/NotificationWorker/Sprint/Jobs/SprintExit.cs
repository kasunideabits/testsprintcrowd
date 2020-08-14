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

    /// <summary>
    /// Exit event notification handling
    /// </summary>
    public class SprintExit : ISprintExit
    {
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="context">db context</param>
        /// <param name="ablyFactory">ably connection factory</param>
        public SprintExit(ScrowdDbContext context, IAblyConnectionFactory ablyFactory, IPushNotificationClient client)
        {
            this.Context = context;
            this.AblyConnectionFactory = ablyFactory;
            this.PushNotificationClient = client;
        }

        private ScrowdDbContext Context { get; }
        private IAblyConnectionFactory AblyConnectionFactory { get; }
        private IPushNotificationClient PushNotificationClient { get; }

        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"><see cref="ExitSprint"> exit data </see></param>
        public void Run(object message = null)
        {
            ExitSprint exitSprint = null;
            if (message != null)
                exitSprint = message as ExitSprint;
            if (exitSprint != null)
            {
                this.AblyMessage(exitSprint);
                this.SendPushNotification(exitSprint);
            }
        }

        private void AblyMessage(ExitSprint exitSprint)
        {
            var notificaitonMsg = ExitNotificationMessageMapper.AblyNotificationMessageMapper(exitSprint);
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + exitSprint.SprintId);
            channel.Publish("Exit", notificaitonMsg);
        }

        private void SendPushNotification(ExitSprint exitSprint)
        {
            if (exitSprint.SprintType == Application.SprintType.PrivateSprint && exitSprint.UserStage != (int)Application.ParticipantStage.COMPLETED)
            {
                // do realy need to send push notification ?
                int notificationId = this.AddToDb(exitSprint, exitSprint.CreatorId);
                var notificationData = ExitNotificationMessageMapper.PushNotificationMessgeMapper(exitSprint);
                var user = this.GetUser(exitSprint.CreatorId);
                var tokens = this.GetTokens(exitSprint.CreatorId);
                var notification = this.GetNotification(user.LanguagePreference);
                var notificationBody = String.Format(notification.Body, exitSprint.Name, exitSprint.SprintName);
                var notificationMessage = this.BuildNotificationMessage(notificationId, notification.Title, notificationBody, tokens, notificationData);
                this.PushNotificationClient.SendMulticaseMessage(notificationMessage);
                this.Context.SaveChanges();
            }
        }

        private dynamic BuildNotificationMessage(int notificationId, string notificationTitle, string notificationBody, List<string> tokens, ExitPushNotificationMesssage notificationData)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)SprintNotificaitonType.LeaveParticipant).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            var message = new PushNotification.PushNotificationMulticastMessageBuilder()
                .Notification(notificationTitle, notificationBody)
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private int AddToDb(ExitSprint exitSprint, int creatorId)
        {
            var sprintNotification = new SprintNotification()
            {
                SprintNotificationType = SprintNotificaitonType.LeaveParticipant,
                UpdatorId = exitSprint.UserId,
                SprintId = exitSprint.SprintId,
                SprintName = exitSprint.SprintName,
                Distance = exitSprint.Distance,
                StartDateTime = exitSprint.StartTime,
                SprintType = exitSprint.SprintType,
                SprintStatus = exitSprint.SprintStatus,
                NumberOfParticipants = exitSprint.NumberOfParticipant,
            };
            var notification = this.Context.Notification.Add(sprintNotification);
            var userNotification = new UserNotification
            {
                SenderId = exitSprint.UserId,
                ReceiverId = creatorId,
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
            var section = translation["sprintLeave"];
            return new SCFireBaseNotificationMessage(section);
        }

    }

    internal class ExitNotification
    {
        /// <summary>
        /// Initialize ExitNotification class
        /// </summary>
        public ExitNotification(int userId, string name, string profilePicture, string code, string city, string country, string countryCode, string sprintName)
        {
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture ?? string.Empty;
            this.Code = code ?? string.Empty;
            this.City = city ?? string.Empty;
            this.Country = country ?? string.Empty;
            this.CountryCode = countryCode ?? string.Empty;
            this.SprintName = sprintName;
        }

        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string ProfilePicture { get; private set; }
        public string Code { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public string CountryCode { get; private set; }

        public string SprintName { get; private set; }
    }

    internal class ExitPushNotificationMesssage
    {
        public ExitPushNotificationMesssage(int sprintId, string sprintName, int distance, DateTime startTime, int numberOfParticipants, SprintType type, SprintStatus status,
            int id, string name, string profilePicture, string code, string city, string country, string countryCode)
        {
            this.Sprint = new ExitSprintInfo(sprintId, sprintName, distance, startTime, numberOfParticipants, type, status);
            this.User = new ExitUserInfo(id, name, profilePicture, code, city, country, countryCode);
        }

        public ExitSprintInfo Sprint { get; }
        public ExitUserInfo User { get; }

        internal sealed class ExitSprintInfo
        {
            public ExitSprintInfo(int id, string name, int distance, DateTime startTime, int numberOfParticipants, SprintType type, SprintStatus status)
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

        internal sealed class ExitUserInfo
        {
            public ExitUserInfo(int id, string name, string profilePicture, string code, string city, string country, string countryCode)
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

    internal static class ExitNotificationMessageMapper
    {
        public static ExitNotification AblyNotificationMessageMapper(ExitSprint exitSprint)
        {
            return new ExitNotification(
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

        public static ExitPushNotificationMesssage PushNotificationMessgeMapper(ExitSprint exitSprint)
        {
            return new ExitPushNotificationMesssage(
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