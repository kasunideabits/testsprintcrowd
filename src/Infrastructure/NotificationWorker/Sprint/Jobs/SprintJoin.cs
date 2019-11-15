namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using System.Collections.Generic;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;

    /// <summary>
    /// join sprint notification handling
    /// </summary>
    public class SprintJoin : SprintNotificationJobBase, ISprintJoin
    {
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="context">db context</param>
        /// <param name="ablyFactory">ably connection factory</param>
        /// <param name="client">push notification client</param>
        public SprintJoin(ScrowdDbContext context, IAblyConnectionFactory ablyFactory, IPushNotificationClient client) : base(context)
        {
            this.AblyConnectionFactory = ablyFactory;
            this.PushNotificationClient = client;
        }

        private IPushNotificationClient PushNotificationClient { get; }
        private IAblyConnectionFactory AblyConnectionFactory { get; }
        private JoinSprint _joinSprint { get; set; }

        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"><see cref="JoinSprint"> join data </see></param>
        public void Run(object message = null)
        {
            JoinSprint joinSprint = message as JoinSprint;
            if (joinSprint != null)
            {
                this._joinSprint = joinSprint;
                this.AblyMessage();
                this.SendPushNotification();
            }
        }

        private void AblyMessage()
        {
            System.Console.WriteLine(this._joinSprint.Name);
        }

        private void SendPushNotification()
        {
            if (this._joinSprint.SprintType == Application.SprintType.PrivateSprint)
            {
                this.PrivateSprintNotification();
            }
            else
            {
                this.PublicSprintNotification();
            }
        }

        private void PrivateSprintNotification()
        {
            var creator = this.GetCreator(this._joinSprint.SprintId);
            var ids = new List<int>() { creator.Id };
            var tokens = this.GetTokens(ids);

            var participant = this.GetParticipant();
            var eventInfo = this.GetEvent();
            var notificationType = this._joinSprint.Accept ? SprintNotificaitonType.InvitationAccept : SprintNotificaitonType.InvitationDecline;
            var notificationId = this.AddToDatabase(eventInfo, participant.Id, ids, notificationType);
            var message = this.BuildNotificationMessage(notificationId, (int)notificationType, tokens, new { User = participant, Sprint = eventInfo });
            this.PushNotificationClient.SendMulticaseMessage(message);
        }

        private void PublicSprintNotification()
        {
            var ids = this.GetFriendIdsInSprint(this._joinSprint.UserId, this._joinSprint.SprintId);
            if (ids.Count > 0)
            {
                var tokens = this.GetTokens(ids);
                var participant = this.GetParticipant();
                var eventInfo = this.GetEvent();
                var notificationId = this.AddToDatabase(eventInfo, participant.Id, ids, SprintNotificaitonType.FriendJoin);

                var message = this.BuildNotificationMessage(notificationId, (int)SprintNotificaitonType.FriendJoin, tokens, new
                {
                    User = participant, Sprint = eventInfo
                });
                this.PushNotificationClient.SendMulticaseMessage(message);
            }
        }

        private NotificationUserInfo GetParticipant()
        {
            var participant = this.GetParticipant(this._joinSprint.UserId);
            return this.SetSprintPariticipant(
                participant.Id,
                participant.Name,
                participant.ProfilePicture,
                participant.Code,
                participant.ColorCode,
                participant.City,
                participant.Country,
                participant.CountryCode);
        }

        private NotificationSprintInfo GetEvent()
        {
            var sprint = this.GetSprint(this._joinSprint.SprintId);
            return this.SetSprintInfo(
                sprint.Id,
                sprint.Name,
                sprint.Distance,
                sprint.StartDateTime,
                sprint.NumberOfParticipants,
                (SprintType)sprint.Type,
                (SprintStatus)sprint.Status);
        }
    }
}