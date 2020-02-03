namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Jobs
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
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
                var participant = this.GetParticipant();
                this.AblyMessage(participant);
                this.SendPushNotification(participant);
            }
        }

        private void AblyMessage(Participant participant)
        {
            // var notificaitonMsg = ExitNotificationMessageMapper.AblyNotificationMessageMapper(this._joinSprint.SprintId);
            var message = new Participant()
            {
                Id = participant.Id,
                Name = participant.Name ?? String.Empty,
                Email = participant.Email ?? String.Empty,
                ProfilePicture = participant.ProfilePicture ?? String.Empty,
                Code = participant.Code ?? String.Empty,
                ColorCode = participant.ColorCode ?? String.Empty,
                City = participant.City ?? String.Empty,
                Country = participant.Country ?? String.Empty,
                CountryCode = participant.CountryCode ?? String.Empty,
            };
            IChannel channel = this.AblyConnectionFactory.CreateChannel("sprint" + this._joinSprint.SprintId);
            channel.Publish("Join", message);
        }

        private void SendPushNotification(Participant participant)
        {
            if (this._joinSprint.SprintType == Application.SprintType.PrivateSprint)
            {
                this.PrivateSprintNotification(participant);
            }
            else
            {
                this.PublicSprintNotification(participant);
            }
        }

        private void PrivateSprintNotification(Participant participant)
        {
            var creator = this.GetCreator(this._joinSprint.SprintId);
            var ids = new List<int>() { creator.Id };
            var tokens = this.GetTokens(ids);
            var translation = this.GetNotification(creator.LanguagePreference);
            var notification = this._joinSprint.Accept ? this.GetInvitationAccept(translation) : this.GetInvitationDecline(translation);
            var notificationBody = String.Format(notification.Body, this._joinSprint.Name);
            var eventInfo = this.GetEvent();
            var notificationType = this._joinSprint.Accept ? SprintNotificaitonType.InvitationAccept : SprintNotificaitonType.InvitationDecline;
            var notificationId = this.AddToDatabase(eventInfo, participant, ids, notificationType);
            var message = this.BuildNotificationMessage(notificationId, notification.Title, notificationBody, tokens, participant, eventInfo, notificationType);
            this.PushNotificationClient.SendMulticaseMessage(message);
        }

        private void PublicSprintNotification(Participant participant)
        {
            var ids = this.GetParticipantsIds();
            var eventInfo = this.GetEvent();
            foreach (var group in ids)
            {
                if (group.Value.Count > 0)
                {
                    var tokens = this.GetTokens(group.Value);
                    Console.WriteLine(JsonConvert.SerializeObject(tokens));
                    var notificationId = this.AddToDatabase(eventInfo, participant, group.Value, SprintNotificaitonType.FriendJoin);
                    Console.WriteLine(group.Key);
                    var translation = this.GetNotification(group.Key);
                    Console.WriteLine($"after transation, {group.Key}");
                    var notification = this.GetFriendJoin(translation);
                    Console.WriteLine($"after transation 1, {JsonConvert.SerializeObject(notification)}, {notification.Body}, {notification.Title}");
                    var notificationBody = String.Format(notification.Body, this._joinSprint.Name);
                    Console.WriteLine($"after transation 2, {notificationBody}");
                    var message = this.BuildNotificationMessage(notificationId, notification.Title, notificationBody, tokens, participant, eventInfo, SprintNotificaitonType.FriendJoin);
                    this.PushNotificationClient.SendMulticaseMessage(message);
                }
            }
        }

        private dynamic BuildNotificationMessage(int notificationId, string title, string body, List<string> tokens, Participant participant, EventInfo eventInfo, SprintNotificaitonType notificationType)
        {
            var data = new Dictionary<string, string>();
            var payload = new
            {
                User = participant,
                Sprint = eventInfo,
            };
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)notificationType).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            var message = new PushNotificationMulticastMessageBuilder()
                .Notification(title, body)
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
                .Where(u => u.UserState == UserState.Active)
                .Select(u => new Participant
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
            return this.Context.Sprint.Select(s => new EventInfo
                {
                    Id = s.Id,
                        Name = s.Name,
                        Distance = s.Distance,
                        StartTime = s.StartDateTime,
                        SprintStatus = (SprintStatus)s.Status,
                        SprintType = (SprintType)s.Type,
                        NumberOfPariticipants = s.NumberOfParticipants
                })
                .FirstOrDefault(s => s.Id == this._joinSprint.SprintId);
        }

        private Dictionary<string, List<int>> GetParticipantsIds()
        {
            Dictionary<string, List<int>> ids = new Dictionary<string, List<int>>();
            var ids1 = this.Context.SprintParticipant
                .Where(s => s.SprintId == this._joinSprint.SprintId && s.User.UserState == UserState.Active)
                .Join(this.Context.Frineds,
                    p => p.UserId,
                    f => f.SharedUserId,
                    ((p, f) =>
                        new { UserId = p.UserId, FriendId = f.AcceptedUserId, Language = f.AcceptedUser.LanguagePreference }))
                .Where(s => s.UserId == this._joinSprint.UserId)
                .GroupBy(s => s.Language,
                    s => s.FriendId,
                    (key, g) => new { Language = key, UserId = g.ToList() })
                .ToDictionary(s => s.Language, s => s.UserId);
            var ids2 = this.Context.SprintParticipant
                .Where(s => s.SprintId == this._joinSprint.SprintId && s.User.UserState == UserState.Active)
                .Join(this.Context.Frineds,
                    p => p.UserId,
                    f => f.AcceptedUserId,
                    ((p, f) =>
                        new { UserId = p.UserId, FriendId = f.SharedUserId, Language = f.SharedUser.LanguagePreference }))
                .Where(s => s.UserId == this._joinSprint.UserId)
                .GroupBy(s => s.Language,
                    s => s.FriendId,
                    (key, g) => new { Language = key, UserId = g.ToList() })
                .ToDictionary(s => s.Language, s => s.UserId);
            ids = ids1;
            foreach (var item in ids2)
            {
                if (ids.ContainsKey(item.Key))
                {
                    var temp = ids [item.Key];
                    temp.AddRange(item.Value);
                    ids [item.Key] = temp;
                }
                else
                {
                    ids.Add(item.Key, item.Value);
                }
            }
            return ids;
        }

        private List<string> GetTokens(List<int> userIds)
        {
            return this.Context.FirebaseToken.Where(f => userIds.Contains(f.User.Id)).Select(f => f.Token).ToList();
        }

        private int AddToDatabase(EventInfo eventInfo, Participant user, List<int> receiverIds, SprintNotificaitonType notificationType)
        {
            List<UserNotification> userNotifications = new List<UserNotification>();
            var sprintNotification = new SprintNotification
            {
                SprintNotificationType = notificationType,
                UpdatorId = user.Id,
                SprintId = eventInfo.Id,
                SprintName = eventInfo.Name,
                Distance = eventInfo.Distance,
                StartDateTime = eventInfo.StartTime,
                SprintType = eventInfo.SprintType,
                SprintStatus = eventInfo.SprintStatus,
                NumberOfParticipants = eventInfo.NumberOfPariticipants
            };
            var notification = this.Context.Notification.Add(sprintNotification);
            receiverIds.ForEach(receiverId =>
            {
                userNotifications.Add(new UserNotification
                {
                    SenderId = user.Id,
                        ReceiverId = receiverId,
                        NotificationId = notification.Entity.Id,
                });
            });
            if (userNotifications.Count > 0)
            {
                this.Context.UserNotification.AddRange(userNotifications);
            }
            this.Context.SaveChanges();
            return notification.Entity.Id;
        }

        private JToken GetNotification(string userLang)
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
            return translation;
        }

        private SCFireBaseNotificationMessage GetInvitationAccept(JToken translation) => new SCFireBaseNotificationMessage(translation ["sprintJoinAccept"]);
        private SCFireBaseNotificationMessage GetInvitationDecline(JToken translation) => new SCFireBaseNotificationMessage(translation ["sprintJoinDecline"]);
        private SCFireBaseNotificationMessage GetFriendJoin(JToken translation) => new SCFireBaseNotificationMessage(translation ["friendJoin"]);

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