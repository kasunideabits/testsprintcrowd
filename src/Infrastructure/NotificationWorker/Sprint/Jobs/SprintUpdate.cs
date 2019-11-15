namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using System.Linq.Expressions;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;

    public class SprintUpdate : SprintNotificationJobBase, ISprintUpdate
    {
        public SprintUpdate(ScrowdDbContext context, IPushNotificationClient client, IAblyConnectionFactory ablyFactory) : base(context)
        {
            this.PushNotificationClient = client;
            this.AblyConnectionFactory = ablyFactory;
        }

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
            var sprint = this.MapSprint(updateSprint);
            var updator = this.MapUpdator(editor);
            var notificationMsgData = new UpdateSprintNotificaitonMessage(updator, sprint);
            var query = GetParitcipantQuery(updateSprint.SprintId, updateSprint.CreatorId);
            var participantIds = this.SprintParticipantIds(query);
            this.UpdateSprintNotification(updateSprint.SprintId, updateSprint.NewSprintName, updateSprint.Distance, updateSprint.StartTime);
            if (participantIds.Count > 0)
            {
                var notificationId = this.AddToDatabase(sprint, updateSprint.CreatorId, participantIds, SprintNotificaitonType.Edit);
                var tokens = this.GetTokens(participantIds);
                var notificationMsg = this.BuildNotificationMessage(notificationId, (int)SprintNotificaitonType.Edit, tokens, notificationMsgData);
                this.PushNotificationClient.SendMulticaseMessage(notificationMsg);
                this.SendAblyMessage(sprint);
            }
            this.SaveChanges();
        }

        private static Expression<Func<SprintParticipant, bool>> GetParitcipantQuery(int sprintId, int creatorId) => s => s.SprintId == sprintId && (s.Stage != ParticipantStage.QUIT || s.Stage != ParticipantStage.DECLINE || s.Stage != ParticipantStage.COMPLETED) && s.UserId != creatorId;

        private NotificationSprintInfo MapSprint(UpdateSprint info)
        {
            return this.SetSprintInfo(info.SprintId, info.OldSprintName, info.Distance, info.StartTime, info.NumberOfParticipant, info.SprintType, info.SprintStatus);
        }

        private NotificationUserInfo MapUpdator(User user)
        {
            return this.SetSprintPariticipant(user.Id, user.Name, user.ProfilePicture, user.Code, user.ColorCode, user.City, user.Country, user.CountryCode);
        }

        private void SendAblyMessage(NotificationSprintInfo message)
        {
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + message.Id);
            channel.Publish("Edit", message);
        }

        internal class UpdateSprintNotificaitonMessage
        {
            public UpdateSprintNotificaitonMessage(NotificationUserInfo user, NotificationSprintInfo sprint)
            {
                this.Sprint = sprint;
                this.EditedBy = user;
            }

            public NotificationSprintInfo Sprint { get; }
            public NotificationUserInfo EditedBy { get; }
        }
    }
}