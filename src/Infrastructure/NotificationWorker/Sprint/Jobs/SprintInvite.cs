using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
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
        private Infrastructure.Persistence.Entities.Sprint Sprint { get; set; }
        private User Inviter { get; set; }
        private User Invitee { get; set; }

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
                this.Sprint = this.GetSprint(this._sprintId);
                this.Inviter = this.GetParticipant(this._inviterId);
                this.Invitee = this.GetParticipant(this._inviteeId);
                if (this.Sprint != null && this.Invitee != null && this.Inviter != null)
                {
                    this.AddToDatabaase();
                    this.SendPushNotification();
                }
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
            var inviter = new
            {
                id = this.Inviter.Id,
                name = this.Inviter.Name,
                email = this.Inviter.Email,
                profilePicture = this.Inviter.ProfilePicture
            };
            var invitee = new
            {
                id = this.Invitee.Id,
                name = this.Invitee.Name,
                email = this.Invitee.Email,
                profilePicture = this.Invitee.ProfilePicture
            };
            var sprint = new
            {
                id = this.Sprint.Id,
                name = this.Sprint.Name,
                distance = this.Sprint.Distance,
                startTime = this.Sprint.StartDateTime,
                numberOfParticipants = this.Sprint.NumberOfParticipants
            };
            var sprintTypeObj = new
            {
                type = SprintNotificaitonType.InvitationRequest,
                createDate = DateTime.UtcNow,
                data = new
                {
                inviter = inviter,
                invitee = invitee,
                sprint = sprint,
                }
            };
            data.Add("SprintType", JsonConvert.SerializeObject(sprintTypeObj));
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
            if (this.Sprint != null)
            {
                SprintNotification sprintNotificaiton = new SprintNotification()
                {
                SenderId = this._inviterId,
                ReceiverId = this._inviteeId,
                Type = SprintNotificaitonType.InvitationRequest,
                UpdatorId = this._inviterId,
                SprintId = this.Sprint.Id,
                SprintName = this.Sprint.Name,
                Distance = this.Sprint.Distance,
                StartDateTime = this.Sprint.StartDateTime,
                SprintType = (SprintType)this.Sprint.Type,
                Status = (SprintStatus)this.Sprint.Status,
                NumberOfParticipants = this.Sprint.NumberOfParticipants
                };
                this.Context.SprintNotifications.Add(sprintNotificaiton);
                this.Context.SaveChanges();
            }

        }

        private Infrastructure.Persistence.Entities.Sprint GetSprint(int sprintId)
        {
            return this.Context.Sprint.Where(s => s.Id == sprintId).FirstOrDefault();
        }

        private List<string> GetTokens()
        {
            return this.Context.FirebaseToken
                .Where(f => f.User.Id == this._inviteeId)
                .Select(f => f.Token)
                .ToList();
        }

        private User GetParticipant(int userId)
        {
            return this.Context.User.FirstOrDefault(u => u.Id == userId);
        }
    }

}