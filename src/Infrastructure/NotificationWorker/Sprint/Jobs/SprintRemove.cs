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
    public class SprintRemove : ISprintRemove
    {
        public SprintRemove(ScrowdDbContext context, IPushNotificationClient client)
        {
            this.Context = context;
            this.PushNotificationClient = client;
        }

        private ScrowdDbContext Context { get; }
        private IPushNotificationClient PushNotificationClient { get; }

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
            var participantIds = this.SprintParticipantIds(removeSprint.SprintId);
            var notificationSprintNotification = RemoveNotificationMessageMapper.SprintRemoveNotificationDbEntry(removeSprint, participantIds);
            this.RemoveOldNotificaiton(removeSprint.SprintId);
            this.AddToDb(notificationSprintNotification);
            var tokens = GetTokens(participantIds);
            var notificationMsg = BuildNotificationMessage(tokens, notificationMsgData);
            this.PushNotificationClient.SendMulticaseMessage(notificationMsg);
        }

        private dynamic BuildNotificationMessage(List<string> tokens, SprintRemoveNotificationMessage notificationData)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)SprintNotificaitonType.Remove).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            var message = new PushNotification.PushNotificationMulticastMessageBuilder()
                .Notification("Sprint Invite Notification", "sprint demo")
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }

        private List<int> SprintParticipantIds(int sprintId)
        {
            return this.Context.SprintParticipant
                .Where(s => s.SprintId == sprintId && s.Stage != ParticipantStage.DECLINE)
                .Select(s => s.UserId)
                .ToList();
        }

        private List<string> GetTokens(List<int> participantIds)
        {
            return this.Context.FirebaseToken
                .Where(f => participantIds.Contains(f.User.Id))
                .Select(f => f.Token).ToList();
        }

        private void AddToDb(List<SprintNotification> notifications)
        {
            this.Context.SprintNotifications.AddRange(notifications);
        }

        private void RemoveOldNotificaiton(int sprintId)
        {
            this.Context.SprintNotifications.Where(n => n.SprintId == sprintId && n.SprintNotificationType == SprintNotificaitonType.Remove);
        }

    }
}

internal sealed class SprintRemoveNotificationMessage
{
    public SprintRemoveNotificationMessage(int userId, string userName, string profilePicture, string code, string colorCode, string city, string country, string countryCode,
        int sprintId, string sprintName, int distance, DateTime startTime, int numberOfPariticipants, SprintType sprintType, SprintStatus sprintStatus)
    {
        this.Sprint = new SprintInfo(sprintId, sprintName, distance, startTime, numberOfPariticipants, sprintType, sprintStatus);
        this.DeletedBy = new RemoverInfo(userId, userName, profilePicture, code, colorCode, city, country, countryCode);

    }

    public SprintInfo Sprint { get; }
    public RemoverInfo DeletedBy { get; }
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

    public static List<SprintNotification> SprintRemoveNotificationDbEntry(RemoveSprint remove, List<int> participantIds)
    {
        List<SprintNotification> sprintNotifications = new List<SprintNotification>();
        participantIds.ForEach(id =>
        {
            sprintNotifications.Add(new SprintNotification
            {
                SenderId = remove.UserId,
                    ReceiverId = id,
                    SprintNotificationType = SprintNotificaitonType.Remove,
                    UpdatorId = remove.UserId,
                    SprintId = remove.SprintId,
                    SprintName = remove.SprintName,
                    Distance = remove.Distance,
                    StartDateTime = remove.StartTime,
                    SprintType = remove.SprintType,
                    SprintStatus = remove.SprintStatus,
                    NumberOfParticipants = remove.NumberOfParticipant
            });
        });
        return sprintNotifications;
    }
}

internal sealed class RemoverInfo
{
    public RemoverInfo(int id, string name, string profilePicture, string code, string colorCode, string city, string country, string countryCode)
    {
        this.Id = id;
        this.Name = name;
        this.ProfilePicture = profilePicture;
        this.Code = code;
        this.ColorCode = colorCode;
        this.City = city;
        this.Country = country;
        this.CountryCode = countryCode;
    }
    public int Id { get; }
    public string Name { get; }
    public string ProfilePicture { get; }
    public string Code { get; }
    public string ColorCode { get; }
    public string City { get; }
    public string Country { get; }
    public string CountryCode { get; }
}

internal sealed class SprintInfo
{
    public SprintInfo(int id, string name, int distance, DateTime startTime, int numberOfPariticipants, SprintType sprintType, SprintStatus sprintStatus)
    {
        this.Id = id;
        this.Name = name;
        this.Distance = distance;
        this.StartTime = startTime;
        this.NumberOfPariticipants = numberOfPariticipants;
        this.SprintType = sprintType;
        this.SprintStatus = sprintStatus;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Distance { get; set; }
    public DateTime StartTime { get; set; }
    public int NumberOfPariticipants { get; set; }
    public SprintType SprintType { get; set; }
    public SprintStatus SprintStatus { get; set; }
}
}