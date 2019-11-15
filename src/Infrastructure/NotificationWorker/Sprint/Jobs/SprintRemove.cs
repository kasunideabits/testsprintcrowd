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
    using static SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs.SprintNotificationJobBase;

    public class SprintRemove : SprintNotificationJobBase, ISprintRemove
    {
        public SprintRemove(ScrowdDbContext context, IPushNotificationClient client, IAblyConnectionFactory ablyFactory) : base(context)
        {
            this.PushNotificationClient = client;
            this.AblyConnectionFactory = ablyFactory;
        }

        private ScrowdDbContext Context { get; }
        private IPushNotificationClient PushNotificationClient { get; }
        private IAblyConnectionFactory AblyConnectionFactory { get; }

        public void Run(object message = null)
        {
            RemoveSprint removeSprint = message as RemoveSprint;
            if (removeSprint != null)
            {
                this.SendPushNotification(removeSprint);
            }
        }

        private void SendPushNotification(RemoveSprint removeSprint)
        {
            var notificationMsgData = RemoveNotificationMessageMapper.SprintRemoveNotificationMessage(removeSprint);
            var particiapntsQuery = GetParitcipantQuery(removeSprint.SprintId, removeSprint.UserId);
            var participantIds = this.SprintParticipantIds(particiapntsQuery);
            this.RemoveOldNotificaiton(removeSprint.SprintId);
            if (participantIds.Count > 0)
            {
                var sprint = this.SetSprintInfo(
                    removeSprint.SprintId,
                    removeSprint.SprintName,
                    removeSprint.Distance,
                    removeSprint.StartTime,
                    removeSprint.NumberOfParticipant,
                    removeSprint.SprintType,
                    removeSprint.SprintStatus
                );
                var notificationId = this.AddToDatabase(sprint, removeSprint.UserId, participantIds, SprintNotificaitonType.Remove);
                this.Context.SaveChanges();
                var tokens = this.GetTokens(participantIds);
                var notificationMsg = this.BuildNotificationMessage(notificationId, (int)SprintNotificaitonType.Remove, tokens, notificationMsgData);
                this.PushNotificationClient.SendMulticaseMessage(notificationMsg);
                this.SendAblyMessage(notificationMsgData.Sprint);
            }
        }

        private void SendAblyMessage(NotificationSprintInfo message)
        {
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + message.Id);
            channel.Publish("Remove", message);
        }

        private static Expression<Func<SprintParticipant, bool>> GetParitcipantQuery(int sprintId, int removerId) => s => s.SprintId == sprintId && s.Stage != ParticipantStage.DECLINE && s.UserId != removerId;
    }

    internal sealed class SprintRemoveNotificationMessage
    {
        public SprintRemoveNotificationMessage(
            int userId, string userName, string profilePicture, string code, string colorCode, string city, string country, string countryCode,
            int sprintId, string sprintName, int distance, DateTime startTime, int numberOfPariticipants, SprintType sprintType, SprintStatus sprintStatus)
        {
            this.Sprint = new NotificationSprintInfo(sprintId, sprintName, distance, startTime, numberOfPariticipants, sprintType, sprintStatus);
            this.DeletedBy = new NotificationUserInfo(userId, userName, profilePicture, code, colorCode, city, country, countryCode);
        }

        public NotificationSprintInfo Sprint { get; }
        public NotificationUserInfo DeletedBy { get; }
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
}