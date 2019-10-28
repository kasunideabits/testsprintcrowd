using System.Collections.Generic;
using System.Linq;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowd.BackEnd.Infrastructure.PushNotification;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    /// <summary>
    /// Implement sprint invite notification
    /// </summary>
    public class SprintInvite : ISprintInvite
    {
        public SprintInvite(ScrowdDbContext context, IPushNotificationClient client)
        {
            this.Context = context;
            this.PushNotificationClient = client;
        }

        private ScrowdDbContext Context { get; }
        private IPushNotificationClient PushNotificationClient { get; }
        private int _sprintId { get; set; }
        private int _inviterId { get; set; }
        private int _inviteeId { get; set; }

        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"></param>
        public void Run(object message = null)
        {
            InviteSprint inviteSprint = message as InviteSprint;
            if (inviteSprint != null)
            {
                this._sprintId = inviteSprint.SprintId;
                this._inviterId = inviteSprint.InviterId;
                this._inviteeId = inviteSprint.InviteeId;
                this.AddToDatabaase();
                this.SendPushNotification();
            }
        }

        private void SendPushNotification()
        {
            var notificationMessage = this.BuildNotificationMessage();
            this.PushNotificationClient.SendMulticaseMessage(notificationMessage);
        }

        private dynamic BuildNotificationMessage()
        {
            var data = new Dictionary<string, string>();
            var tokens = this.GetTokens();
            var message = new PushNotificationMulticastMessageBuilder()
                .Notification("Sprint Invite Notification", "sprint demo")
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private void AddToDatabaase()
        {
            var sprint = this.GetSprint();
            var sprintNotificaiton = new SprintNotification()
            {
                SenderId = this._inviterId,
                ReceiverId = this._inviteeId,
                Type = SprintNotificaitonType.InvitationRequest,
                UpdatorId = this._inviterId,
                SprintId = sprint.Id,
                SprintName = sprint.Name,
                Distance = sprint.Distance,
                StartDateTime = sprint.StartDateTime,
                SprintType = (SprintType)sprint.Type,
                Status = (SprintStatus)sprint.Status,
                NumberOfParticipants = sprint.NumberOfParticipants
            };
            this.Context.SprintNotifications.Add(sprintNotificaiton);
            this.Context.SaveChanges();

        }

        private Infrastructure.Persistence.Entities.Sprint GetSprint()
        {
            return this.Context.Sprint.Where(s => s.Id == this._sprintId).FirstOrDefault();
        }

        private List<string> GetTokens()
        {
            return this.Context.FirebaseToken
                .Where(f => f.User.Id == this._inviteeId)
                .Select(f => f.Token)
                .ToList();
        }

    }

}