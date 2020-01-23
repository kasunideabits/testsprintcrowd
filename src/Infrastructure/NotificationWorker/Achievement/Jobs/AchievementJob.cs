using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement.Dtos;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowd.BackEnd.Infrastructure.PushNotification;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement.Jobs
{
    public class AchievementJob : IAchievementJob
    {
        public AchievementJob(ScrowdDbContext context, IPushNotificationClient client)
        {
            this.AchievementJobRepo = new AchievementJobRepo(context);
            this.Client = client;
        }

        private AchievementJobRepo AchievementJobRepo { get; }
        private IPushNotificationClient Client { get; }

        public void Run(object message = null)
        {
            AchievementMessage achievement = message as AchievementMessage;
            if (achievement != null)
            {
                var notificationId = this.AchievementJobRepo.AddNotification((AchievementType)achievement.Type, achievement.AchievedOn);
                var systemUser = this.AchievementJobRepo.GetSystemUser();
                this.AchievementJobRepo.AddUserNotification(systemUser.Id, achievement.UserId, notificationId);
                var tokens = this.AchievementJobRepo.GetTokens(achievement.UserId);
                this.AchievementJobRepo.SaveChanges();
                var notificationMessagePayload = new AchievmentMessageDto((AchievementType)achievement.Type, achievement.AchievedOn);
                var notificationMessage = this.BuildNotification(notificationId, notificationMessagePayload, tokens);
                this.Client.SendMulticaseMessage(notificationMessage);
            }
        }

        private dynamic BuildNotification(int notificationId, AchievmentMessageDto notificationData, List<string> tokens)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "AchievementType");
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            var message = new PushNotification.PushNotificationMulticastMessageBuilder()
                .Message(data)
                .Tokens(tokens)
                .Build();
            return message;
        }
    }

    internal class AchievmentMessageDto
    {
        public AchievmentMessageDto(AchievementType type, DateTime achievedOn)
        {
            this.Type = type;
            this.AchievedOn = achievedOn;
        }

        public AchievementType Type { get; }
        public DateTime AchievedOn { get; }

    }
}