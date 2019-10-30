namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;

    /// <summary>
    /// join sprint notification handling
    /// </summary>
    public class SprintJoin : ISprintJoin
    {
        /// <summary>
        /// Initialize class
        /// </summary>
        /// <param name="context">db context</param>
        /// <param name="ablyFactory">ably connection factory</param>
        public SprintJoin(ScrowdDbContext context, IAblyConnectionFactory ablyFactory, IPushNotificationClient client)
        {
            this.Context = context;
            this.AblyConnectionFactory = ablyFactory;
            this.PushNotificationClient = client;
        }

        private ScrowdDbContext Context { get; }
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
            this.AddToDatabase(eventInfo, participant, ids, notificationType);
            var message = this.BuildNotificationMessage(tokens, participant, eventInfo, notificationType);
            this.PushNotificationClient.SendMulticaseMessage(message);
        }

        private void PublicSprintNotification()
        {
            var ids = this.GetParticipantsIds();
            var tokens = this.GetTokens(ids);
            var participant = this.GetParticipant();
            var eventInfo = this.GetEvent();
            var message = this.BuildNotificationMessage(tokens, participant, eventInfo, SprintNotificaitonType.FriendJoin);
            this.AddToDatabase(eventInfo, participant, ids, SprintNotificaitonType.FriendJoin);
            this.PushNotificationClient.SendMulticaseMessage(message);
        }

        private dynamic BuildNotificationMessage(List<string> tokens, Participant participant, EventInfo eventInfo, SprintNotificaitonType notificationType)
        {
            var data = new Dictionary<string, string>();
            var sprintTypeObj = new
            {
                type = notificationType,
                createDate = DateTime.UtcNow,
                data = new
                {
                User = participant,
                Sprint = eventInfo,
                }
            };
            data.Add("SprintType", JsonConvert.SerializeObject(sprintTypeObj));
            var message = new PushNotificationMulticastMessageBuilder()
                .Notification("Sprint Invite Notification", "sprint demo")
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private User GetCreator(int sprintId)
        {
            return this.Context.Sprint
                .Include(s => s.CreatedBy)
                .Where(s => s.Id == sprintId).Select(s => s.CreatedBy).FirstOrDefault();
        }

        private Participant GetParticipant()
        {
            return this.Context.User
                .Select(u => new Participant()
                {
                    Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        ProfilePicture = u.ProfilePicture,
                        Code = u.Code,
                        ColorCode = u.ColorCode,
                        City = u.City,
                        Country = u.Country,
                        CountryCode = u.CountryCode
                })
                .FirstOrDefault(u => u.Id == this._joinSprint.UserId);
        }

        private EventInfo GetEvent()
        {
            return this.Context.Sprint.Select(s => new EventInfo()
                {
                    Id = s.Id,
                        Name = s.Name,
                        Distance = s.Distance,
                        StartTime = s.StartDateTime,
                        SprintStatus = (SprintStatus)s.Status,
                        SprintType = (SprintType)s.Type
                })
                .FirstOrDefault();
        }

        private List<int> GetParticipantsIds()
        {
            var ids = from v in this.Context.SprintParticipant
            from b in this.Context.Frineds
            where v.UserId == b.SharedUserId || v.UserId == b.AcceptedUserId
            select v.UserId;
            return ids.ToList();
        }

        private List<string> GetTokens(List<int> userIds)
        {
            return this.Context.FirebaseToken.Where(f => userIds.Contains(f.User.Id)).Select(f => f.Token).ToList();
        }

        private void AddToDatabase(EventInfo eventInfo, Participant user, List<int> receiverIds, SprintNotificaitonType notificationType)
        {
            List<SprintNotification> notifications = new List<SprintNotification>();
            receiverIds.ForEach(receiverId =>
            {
                notifications.Add(new SprintNotification()
                {
                    SenderId = user.Id,
                        ReceiverId = receiverId,
                        Type = notificationType,
                        UpdatorId = user.Id,
                        SprintId = eventInfo.Id,
                        SprintName = eventInfo.Name,
                        Distance = eventInfo.Distance,
                        StartDateTime = eventInfo.StartTime,
                        SprintType = eventInfo.SprintType,
                        Status = eventInfo.SprintStatus,
                        NumberOfParticipants = eventInfo.NumberOfPariticipants
                });
            });
            if (notifications.Count > 0)
            {
                this.Context.SprintNotifications.AddRange(notifications);
                this.Context.SaveChanges();
            }
        }

        internal class Participant
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string ProfilePicture { get; set; }
            public string Code { get; set; }
            public string ColorCode { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string CountryCode { get; set; }

        }

        internal class EventInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Distance { get; set; }
            public DateTime StartTime { get; set; }
            public int NumberOfPariticipants { get; set; }
            public SprintType SprintType { get; set; }
            public SprintStatus SprintStatus { get; set; }
        }

    }
}