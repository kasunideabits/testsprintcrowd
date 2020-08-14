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

    public class SprintRemove : ISprintRemove
    {
        public SprintRemove(ScrowdDbContext context, IPushNotificationClient client, IAblyConnectionFactory ablyFactory)
        {
            this.Context = context;
            this.PushNotificationClient = client;
            this.AblyConnectionFactory = ablyFactory;
        }

        private ScrowdDbContext Context { get; }
        private IPushNotificationClient PushNotificationClient { get; }
        private IAblyConnectionFactory AblyConnectionFactory { get; }

        public void Run(object message = null)
        {
            RemoveSprint removeSprint = null;
            if (message != null)
                removeSprint = message as RemoveSprint;
            if (removeSprint != null)
            {
                this.SendPushNotification(removeSprint);
            }
        }

        private void SendPushNotification(RemoveSprint removeSprint)
        {
            var notificationMsgData = RemoveNotificationMessageMapper.SprintRemoveNotificationMessage(removeSprint);
            var participantIds = this.SprintParticipantIds(removeSprint.SprintId, removeSprint.UserId);
            this.RemoveOldNotificaiton(removeSprint.SprintId);
            foreach (var item in participantIds)
            {
                if (item.Value.Count > 0)
                {
                    var notificationId = this.AddToDb(removeSprint, item.Value);
                    this.Context.SaveChanges();
                    var tokens = this.GetTokens(item.Value);
                    var notification = this.GetNotification(item.Key);
                    var notificationBody = String.Format(notification.Body, removeSprint.SprintName, removeSprint.Name);
                    var notificationMsg = this.BuildNotificationMessage(notificationId, notification.Title, notificationBody, tokens, notificationMsgData);
                    this.PushNotificationClient.SendMulticaseMessage(notificationMsg);
                    this.SendAblyMessage(notificationMsgData.Sprint);
                }
            }
        }

        private void SendAblyMessage(SprintInfo message)
        {
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + message.Id);
            channel.Publish("Remove", message);
        }

        private dynamic BuildNotificationMessage(int notificationId, string title, string body, List<string> tokens, SprintRemoveNotificationMessage notificationData)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)SprintNotificaitonType.Remove).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            var message = new PushNotification.PushNotificationMulticastMessageBuilder()
                .Notification(title, body)
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
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
            var section = translation["sprintRemove"];
            return new SCFireBaseNotificationMessage(section);
        }

        private Dictionary<string, List<int>> SprintParticipantIds(int sprintId, int removerId)
        {
            return this.Context.SprintParticipant
                .Where(s =>
                    s.SprintId == sprintId &&
                    (s.Stage == ParticipantStage.JOINED || s.Stage == ParticipantStage.MARKED_ATTENDENCE) &&
                    s.UserId != removerId &&
                    s.User.UserState == UserState.Active)
                .Select(s => new { UserId = s.UserId, Language = s.User.LanguagePreference })
                .GroupBy(s => s.Language,
                    s => s.UserId,
                    (key, g) => new { Language = key, UserId = g.ToList() })
                .ToDictionary(s => s.Language, s => s.UserId);
        }

        private List<string> GetTokens(List<int> participantIds)
        {
            return this.Context.FirebaseToken
                .Where(f => participantIds.Contains(f.User.Id))
                .Select(f => f.Token).ToList();
        }

        private void RemoveOldNotificaiton(int sprintId)
        {
            var toDeleteNotifications = this.Context.SprintNotifications.Where(n => n.SprintId == sprintId && n.SprintNotificationType != SprintNotificaitonType.Remove);
            this.Context.RemoveRange(toDeleteNotifications);
        }

        private int AddToDb(RemoveSprint remove, List<int> participantIds)
        {
            List<UserNotification> userNotifications = new List<UserNotification>();
            var sprintNotification = new SprintNotification()
            {
                SprintNotificationType = SprintNotificaitonType.Remove,
                UpdatorId = remove.UserId,
                SprintId = remove.SprintId,
                SprintName = remove.SprintName,
                Distance = remove.Distance,
                StartDateTime = remove.StartTime,
                SprintType = remove.SprintType,
                SprintStatus = remove.SprintStatus,
                NumberOfParticipants = remove.NumberOfParticipant
            };
            var notification = this.Context.Notification.Add(sprintNotification);
            participantIds.ForEach(id =>
            {
                userNotifications.Add(new UserNotification
                {
                    SenderId = remove.UserId,
                    ReceiverId = id,
                    NotificationId = notification.Entity.Id,
                });

            });
            this.Context.UserNotification.AddRange(userNotifications);
            return notification.Entity.Id;
        }

    }

    internal sealed class SprintRemoveNotificationMessage
    {
        public SprintRemoveNotificationMessage(int userId, string userName, string profilePicture, string code, string colorCode, string city, string country, string countryCode,
            int sprintId, string sprintName, int distance, DateTime startTime, int numberOfPariticipants, SprintType sprintType, SprintStatus sprintStatus)
        {
            this.Sprint = new SprintInfo(sprintId, sprintName, distance, startTime, numberOfPariticipants, sprintType, sprintStatus);
            this.DeletedBy = new RemoverInfo(userId, userName, profilePicture, code, colorCode, city, country, countryCode);

        }

        public SprintInfo Sprint { get; }
        public RemoverInfo DeletedBy { get; }
    }

    internal static class RemoveNotificationMessageMapper
    {
        public static SprintRemoveNotificationMessage SprintRemoveNotificationMessage(RemoveSprint remove)
        {
            return new SprintRemoveNotificationMessage(
                remove.UserId,
                remove.Name,
                remove.ProfilePicture,
                remove.Code,
                remove.ColorCode,
                remove.City,
                remove.Country,
                remove.CountryCode,
                remove.SprintId,
                remove.SprintName,
                remove.Distance,
                remove.StartTime,
                remove.NumberOfParticipant,
                remove.SprintType,
                remove.SprintStatus);
        }
    }

    internal sealed class RemoverInfo
    {
        public RemoverInfo(int id, string name, string profilePicture, string code, string colorCode, string city, string country, string countryCode)
        {
            this.Id = id;
            this.Name = name;
            this.ProfilePicture = profilePicture ?? string.Empty;
            this.Code = code ?? string.Empty; ;
            this.ColorCode = colorCode ?? string.Empty; ;
            this.City = city ?? string.Empty; ;
            this.Country = country ?? string.Empty; ;
            this.CountryCode = countryCode ?? string.Empty; ;
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

    internal sealed class SprintInfo
    {
        public SprintInfo(int id, string name, int distance, DateTime startTime, int numberOfPariticipants, SprintType sprintType, SprintStatus sprintStatus)
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.StartTime = startTime;
            this.NumberOfPariticipants = numberOfPariticipants;
            this.SprintType = sprintType;
            this.SprintStatus = sprintStatus;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public DateTime StartTime { get; set; }
        public int NumberOfPariticipants { get; set; }
        public SprintType SprintType { get; set; }
        public SprintStatus SprintStatus { get; set; }
    }
}