using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Domain.SprintParticipant;
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
        public SprintInvite(ScrowdDbContext context, IPushNotificationClient client, ISprintParticipantRepo sprintParticipantRepo)
        {
            this.Context = context;
            this.PushNotificationClient = client;
            this.SprintParticipantRepo = sprintParticipantRepo;
        }

        private ScrowdDbContext Context { get; }
        private IPushNotificationClient PushNotificationClient { get; }
        private int _sprintId { get; set; }
        private int _inviterId { get; set; }
        private int _inviteeId { get; set; }
        private Infrastructure.Persistence.Entities.Sprint Sprint { get; set; }
        private User Inviter { get; set; }
        private User Invitee { get; set; }
        private ISprintParticipantRepo SprintParticipantRepo { get; }
        private int ParticipantUserId { get; set; }
        /// <summary>
        /// Run notification logic
        /// </summary>
        /// <param name="message"></param>
        public void Run(object message = null)
        {
            InviteSprint inviteSprint = null;
            if (message != null)
                inviteSprint = message as InviteSprint;
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
                    var notificationId = this.AddToDatabaase();
                    this.ParticipantUserId = this.Invitee.Id;
                    if (notificationId != -1)
                    {
                        this.SendPushNotification(notificationId);
                    }
                }
            }
        }

        private void SendPushNotification(int notificationId)
        {
            var notificationMessage = this.BuildNotificationMessage(notificationId);
            this.PushNotificationClient.SendMulticaseMessage(notificationMessage);
        }

        private dynamic BuildNotificationMessage(int notificationId)
        {
            var data = new Dictionary<string, string>();
            var inviter = new
            {
                Id = this.Inviter.Id,
                Name = this.Inviter.Name,
                Email = this.Inviter.Email,
                ProfilePicture = this.Inviter.ProfilePicture,
                Code = this.Inviter.Code,
                ColorCode = this.Inviter.ColorCode,
                City = this.Inviter.City,
                Country = this.Inviter.Country,
                CountryCode = this.Inviter.CountryCode,

            };
            var invitee = new
            {
                Id = this.Invitee.Id,
                Name = this.Invitee.Name,
                Email = this.Invitee.Email,
                ProfilePicture = this.Invitee.ProfilePicture,
                Code = this.Invitee.Code,
                ColorCode = this.Invitee.ColorCode,
                City = this.Invitee.City,
                Country = this.Invitee.Country,
                CountryCode = this.Invitee.CountryCode,
            };
            var sprint = new
            {
                Id = this.Sprint.Id,
                Name = this.Sprint.Name,
                Distance = this.Sprint.Distance,
                StartTime = this.Sprint.StartDateTime,
                NumberOfParticipants = this.Sprint.NumberOfParticipants,
                SprintStatus = this.Sprint.Status,
                SprintType = this.Sprint.Type
            };
            var payload = new
            {
                Inviter = inviter,
                Invitee = invitee,
                Sprint = sprint,
            };
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)SprintNotificaitonType.InvitationRequest).ToString());
            data.Add("CreateDate", string.Format("{0:yyyy-MM-ddTHH:mm:ss.FFFZ}", DateTime.UtcNow));
            data.Add("Data", JsonConvert.SerializeObject(payload));

            int badge = this.SprintParticipantRepo != null ? this.SprintParticipantRepo.GetParticipantUnreadNotificationCount(this.ParticipantUserId) : 0;
            data.Add("Count", badge.ToString());

            var user = this.GetUser(this._inviteeId);
            var notification = this.GetNotification(user.LanguagePreference);
            var notificationBody = String.Format(notification.Body, inviter.Name, sprint.Name);
            var tokens = this.GetTokens();
            var message = new PushNotificationMulticastMessageBuilder(this.SprintParticipantRepo, this.ParticipantUserId)
                .Notification(notification.Title, notificationBody)
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private User GetUser(int userId)
        {
            return this.Context.User.FirstOrDefault(u => u.Id == userId);
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
            var section = translation ["sprintInvite"];
            return new SCFireBaseNotificationMessage(section);
        }

        private int AddToDatabaase()
        {
            if (this.Sprint != null)
            {
                var sprintNotificaiton = new SprintNotification
                {
                SprintNotificationType = SprintNotificaitonType.InvitationRequest,
                UpdatorId = this._inviterId,
                SprintId = this.Sprint.Id,
                SprintName = this.Sprint.Name,
                Distance = this.Sprint.Distance,
                StartDateTime = this.Sprint.StartDateTime,
                SprintType = (SprintType)this.Sprint.Type,
                SprintStatus = (SprintStatus)this.Sprint.Status,
                NumberOfParticipants = this.Sprint.NumberOfParticipants
                };
                var result = this.Context.SprintNotifications.Add(sprintNotificaiton);
                this.Context.SaveChanges();
                var userNotification = new UserNotification
                {
                    SenderId = this._inviterId,
                    ReceiverId = this._inviteeId,
                    NotificationId = result.Entity.Id,
                    BadgeValue = 1,
                };
                this.Context.UserNotification.Add(userNotification);
                this.Context.SaveChanges();
                return result.Entity.Id;
            }
            return -1;
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