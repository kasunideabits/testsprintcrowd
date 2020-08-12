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

    public class SprintUpdate : ISprintUpdate
    {
        public SprintUpdate(ScrowdDbContext context, IPushNotificationClient client, IAblyConnectionFactory ablyFactory)
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
            {
                UpdateSprint editSprint = message as UpdateSprint;
                if (editSprint != null)
                {
                    this.SendPushNotification(editSprint);
                }
            }
        }

        private void SendPushNotification(UpdateSprint updateSprint)
        {
            var editor = this.GetParticipant(updateSprint.CreatorId);
            var notificationMsgData = UpdateNotificationMessageMapper.UpdateMessage(editor, updateSprint);
            var participantIds = this.SprintParticipantIds(updateSprint.SprintId, updateSprint.CreatorId);
            this.UpdateSprintNotification(updateSprint);
            foreach (var item in participantIds)
            {
                if (item.Value.Count > 0)
                {
                    var notificationId = this.AddToDb(updateSprint, item.Value, updateSprint.CreatorId);
                    var tokens = this.GetTokens(item.Value);
                    var notification = this.GetNotification(item.Key);
                    var notificationBody = String.Format(notification.Body, updateSprint.OldSprintName, editor.Name);
                    var notificationMsg = this.BuildNotificationMessage(notificationId, notification.Title, notificationBody, tokens, notificationMsgData);
                    this.PushNotificationClient.SendMulticaseMessage(notificationMsg);
                    this.SendAblyMessage(notificationMsgData.Sprint);
                }
            }

            this.Context.SaveChanges();
        }

        private dynamic BuildNotificationMessage(int notificationId, string title, string body, List<string> tokens, UpdateSprintNotificaitonMessage notificationData)
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
            var section = translation["sprintEdit"];
            return new SCFireBaseNotificationMessage(section);
        }

        private void SendAblyMessage(UpdatedSprintInfo message)
        {
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + message.Id);
            channel.Publish("Edit", message);
        }

        private Dictionary<string, List<int>> SprintParticipantIds(int sprintId, int creatorId)
        {
            return this.Context.SprintParticipant
                .Where(s =>
                    s.SprintId == sprintId &&
                    s.User.UserState == UserState.Active &&
                    (s.Stage == ParticipantStage.JOINED || s.Stage == ParticipantStage.MARKED_ATTENDENCE) &&
                    s.UserId != creatorId)
                .Select(s => new { UserId = s.UserId, Lanugage = s.User.LanguagePreference })
                .GroupBy(s => s.Lanugage,
                    s => s.UserId,
                    (key, g) => new { Language = key, UserId = g.ToList() })
                .ToDictionary(s => s.Language, s => s.UserId);
        }

        private User GetParticipant(int userId) => this.Context.User.FirstOrDefault(u => u.Id == userId);

        private int AddToDb(UpdateSprint edit, List<int> participantIds, int creatorId)
        {

            List<UserNotification> userNotifications = new List<UserNotification>();
            var sprintNotification = new SprintNotification()
            {
                SprintNotificationType = SprintNotificaitonType.Edit,
                UpdatorId = creatorId,
                SprintId = edit.SprintId,
                SprintName = edit.OldSprintName,
                Distance = edit.Distance,
                StartDateTime = edit.StartTime,
                SprintType = edit.SprintType,
                SprintStatus = edit.SprintStatus,
                NumberOfParticipants = edit.NumberOfParticipant
            };
            var notification = this.Context.Notification.Add(sprintNotification);
            participantIds.ForEach(id =>
            {
                userNotifications.Add(new UserNotification
                {
                    SenderId = creatorId,
                    ReceiverId = id,
                    NotificationId = notification.Entity.Id,
                });

            });
            this.Context.UserNotification.AddRange(userNotifications);
            return notification.Entity.Id;
        }

        private void UpdateSprintNotification(UpdateSprint edit)
        {
            //List<SprintNotification> existingNotification = this.Context.SprintNotifications.Where(s => s.SprintId == edit.SprintId).ToList();
            //existingNotification.ForEach(n =>
            //{
            //    n.SprintName = edit.NewSprintName;
            //    n.Distance = edit.Distance;
            //    n.StartDateTime = edit.StartTime;
            //});
            //this.Context.SprintNotifications.UpdateRange(existingNotification);

            List<SprintNotification> existingNotification = this.Context.SprintNotifications.Where(s => s.SprintId == edit.SprintId).ToList();
            SprintNotification entitySprintNotification = existingNotification.OrderByDescending(x => x.Id).FirstOrDefault();

            if (existingNotification.Count == 1 && entitySprintNotification.SprintNotificationType != SprintNotificaitonType.Edit)
            {
                var sprintNotificaitonOne = new SprintNotification
                {
                    SprintNotificationType = SprintNotificaitonType.Edit,
                    UpdatorId = entitySprintNotification.UpdatorId,
                    SprintId = entitySprintNotification.SprintId,
                    SprintName = entitySprintNotification.SprintName,
                    Distance = entitySprintNotification.Distance,
                    StartDateTime = entitySprintNotification.StartDateTime,
                    SprintType = entitySprintNotification.SprintType,
                    SprintStatus = entitySprintNotification.SprintStatus,
                    NumberOfParticipants = entitySprintNotification.NumberOfParticipants
                };

                this.Context.SprintNotifications.Add(sprintNotificaitonOne);
            }

            if (entitySprintNotification != null)
            {
                var sprintNotificaiton = new SprintNotification
                {
                    SprintNotificationType = SprintNotificaitonType.Edit,
                    UpdatorId = entitySprintNotification.UpdatorId,
                    SprintId = entitySprintNotification.SprintId,
                    SprintName = edit.NewSprintName,
                    Distance = edit.Distance,
                    StartDateTime = edit.StartTime,
                    SprintType = entitySprintNotification.SprintType,
                    SprintStatus = entitySprintNotification.SprintStatus,
                    NumberOfParticipants = entitySprintNotification.NumberOfParticipants
                };

                this.Context.SprintNotifications.Add(sprintNotificaiton);
            }

            //if (entitySprintNotification != null)
            //{
            //    entitySprintNotification.SprintName = edit.NewSprintName;
            //    entitySprintNotification.Distance = edit.Distance;
            //    entitySprintNotification.StartDateTime = edit.StartTime;

            //    this.Context.SprintNotifications.UpdateRange(entitySprintNotification);
            //}
        }

        private List<string> GetTokens(List<int> participantIds)
        {
            return this.Context.FirebaseToken
                .Where(f => participantIds.Contains(f.User.Id))
                .Select(f => f.Token).ToList();
        }

        internal sealed class UpdateSprintNotificaitonMessage
        {
            public UpdateSprintNotificaitonMessage(
                int sprintId,
                string sprintName,
                int distance,
                DateTime startTime,
                int numberOfParticipants,
                SprintType sprintType,
                SprintStatus sprintStatus,
                int editorId,
                string editorName,
                string editorProfilePicture,
                string editorEmail,
                string editorCode,
                string editorCity,
                string editorCountry,
                string editorCountryCode)
            {
                this.Sprint = new UpdatedSprintInfo(sprintId, sprintName, distance, startTime, numberOfParticipants, sprintType, sprintStatus);
                this.EditedBy = new EditorInfo(editorId, editorName, editorProfilePicture, editorEmail, editorCode, editorCity, editorCountry, editorCountryCode);
            }

            public UpdatedSprintInfo Sprint { get; }
            public EditorInfo EditedBy { get; }

        }

        internal class EditorInfo
        {
            public EditorInfo(int id, string name, string profilePicture, string email, string code, string city, string country, string countryCode)
            {
                this.Id = id;
                this.Name = name;
                this.ProfilePicture = profilePicture ?? string.Empty;
                this.Email = email;
                this.Code = code ?? string.Empty;
                this.City = city ?? string.Empty;
                this.Country = country ?? string.Empty;
                this.CountryCode = countryCode ?? string.Empty;
            }

            public int Id { get; }
            public string Name { get; }
            public string ProfilePicture { get; }
            public string Email { get; }
            public string Code { get; }
            public string ColorCode { get; }
            public string City { get; }
            public string Country { get; }
            public string CountryCode { get; }
        }

        internal sealed class UpdatedSprintInfo
        {
            public UpdatedSprintInfo(int sprintId, string sprintName, int distance, DateTime startTime, int numberOfParticipants, SprintType sprintType, SprintStatus sprintStatus)
            {
                this.Id = sprintId;
                this.Name = sprintName;
                this.Distance = distance;
                this.StartTime = startTime;
                this.NumberOfParticipants = numberOfParticipants;
                this.SprintStatus = sprintStatus;
                this.SprintType = sprintType;
            }
            public int Id { get; }
            public string Name { get; }
            public int Distance { get; }
            public DateTime StartTime { get; }
            public int NumberOfParticipants { get; }
            public SprintStatus SprintStatus { get; }
            public SprintType SprintType { get; }
        }

        internal static class UpdateNotificationMessageMapper
        {
            public static UpdateSprintNotificaitonMessage UpdateMessage(User editor, UpdateSprint edit)
            {
                return new UpdateSprintNotificaitonMessage(
                    edit.SprintId,
                    edit.OldSprintName,
                    edit.Distance,
                    edit.StartTime,
                    edit.NumberOfParticipant,
                    edit.SprintType,
                    edit.SprintStatus,
                    editor.Id,
                    editor.Name,
                    editor.ProfilePicture,
                    editor.Email,
                    editor.Code,
                    editor.City,
                    editor.Country,
                    editor.CountryCode);
            }
        }
    }
}