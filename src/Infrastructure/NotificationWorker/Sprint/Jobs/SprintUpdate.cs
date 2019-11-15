namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
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
            var notificationMsgData = UpdateNotificationMessageMapper.UpdateMessage(updateSprint);
            var participantIds = this.SprintParticipantIds(updateSprint.SprintId, updateSprint.CreatorId);
            this.UpdateSprintNotification(updateSprint);
            if (participantIds.Count > 0)
            {
                var notificationId = this.AddToDb(updateSprint, participantIds, updateSprint.CreatorId);
                var tokens = this.GetTokens(participantIds);
                var notificationMsg = this.BuildNotificationMessage(notificationId, tokens, notificationMsgData);
                this.PushNotificationClient.SendMulticaseMessage(notificationMsg);
                this.SendAblyMessage(notificationMsgData.Sprint);
            }
            this.Context.SaveChanges();
        }

        private dynamic BuildNotificationMessage(int notificationId, List<string> tokens, UpdateSprintNotificaitonMessage notificationData)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)SprintNotificaitonType.Edit).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            var message = new PushNotification.PushNotificationMulticastMessageBuilder()
                .Notification("Sprint Invite Notification", "sprint demo")
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private void SendAblyMessage(UpdatedSprintInfo message)
        {
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + message.Id);
            channel.Publish("Edit", message);
        }

        private List<int> SprintParticipantIds(int sprintId, int creatorId)
        {
            return this.Context.SprintParticipant
                .Where(s => s.SprintId == sprintId && (s.Stage != ParticipantStage.QUIT || s.Stage != ParticipantStage.DECLINE || s.Stage != ParticipantStage.COMPLETED) && s.UserId != creatorId)
                .Select(s => s.UserId)
                .ToList();
        }

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
            List<SprintNotification> existingNotification = this.Context.SprintNotifications.Where(s => s.SprintId == edit.SprintId).ToList();
            existingNotification.ForEach(n =>
            {
                n.SprintName = edit.NewSprintName;
                n.Distance = edit.Distance;
                n.StartDateTime = edit.StartTime;
            });
            this.Context.SprintNotifications.UpdateRange(existingNotification);
        }

        private List<string> GetTokens(List<int> participantIds)
        {
            return this.Context.FirebaseToken
                .Where(f => participantIds.Contains(f.User.Id))
                .Select(f => f.Token).ToList();
        }

        internal sealed class UpdateSprintNotificaitonMessage
        {
            public UpdateSprintNotificaitonMessage(int sprintId, string sprintName, int distance, DateTime startTime, int numberOfParticipants, SprintType sprintType, SprintStatus sprintStatus)
            {
                this.Sprint = new UpdatedSprintInfo(sprintId, sprintName, distance, startTime, numberOfParticipants, sprintType, sprintStatus);
            }

            public UpdatedSprintInfo Sprint { get; }

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
            public static UpdateSprintNotificaitonMessage UpdateMessage(UpdateSprint edit)
            {
                return new UpdateSprintNotificaitonMessage(
                    edit.SprintId,
                    edit.OldSprintName,
                    edit.Distance,
                    edit.StartTime,
                    edit.NumberOfParticipant,
                    edit.SprintType,
                    edit.SprintStatus);
            }
        }
    }
}