using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SprintCrowd.BackEnd.Domain.SprintParticipant;
using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement.Dtos;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowd.BackEnd.Infrastructure.PushNotification;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Achievement.Jobs
{
    public class AchievementJob : IAchievementJob
    {
        public AchievementJob(ScrowdDbContext context, IPushNotificationClient client, ISprintParticipantRepo sprintParticipantRepo)
        {
            this.AchievementJobRepo = new AchievementJobRepo(context);
            this.Client = client;
            this.SprintParticipantRepo = sprintParticipantRepo;
        }

        private AchievementJobRepo AchievementJobRepo { get; }
        private IPushNotificationClient Client { get; }

        private ISprintParticipantRepo SprintParticipantRepo { get; }
        private int ParticipantUserId { get; set; }

        public void Run(object message = null)
        {
            AchievementMessage achievement = null;
            if (message != null)
                achievement = message as AchievementMessage;
            if (achievement != null)
            {
                var notificationId = this.AchievementJobRepo.AddNotification((AchievementType)achievement.Type, achievement.AchievedOn);
                var systemUser = this.AchievementJobRepo.GetSystemUser();
                var participant = this.AchievementJobRepo.GetUser(achievement.UserId);
                this.ParticipantUserId = achievement.UserId;
                this.AchievementJobRepo.AddUserNotification(systemUser.Id, achievement.UserId, notificationId);
                var notification = new AchievementTranslation(participant.LanguagePreference).Get((AchievementType)achievement.Type);
                var tokens = this.AchievementJobRepo.GetTokens(achievement.UserId);
                this.AchievementJobRepo.SaveChanges();
                var notificationMessagePayload = new AchievmentMessageDto((AchievementType)achievement.Type, achievement.AchievedOn);
                var notificationMessage = this.BuildNotification(notificationId, notificationMessagePayload, notification, tokens);
                this.Client.SendMulticaseMessage(notificationMessage);
            }
        }

        private dynamic BuildNotification(
            int notificationId,
            AchievmentMessageDto notificationData,
            SCFireBaseNotificationMessage notification,
            List<string> tokens)
        {
            var data = new Dictionary<string, string>();
            var payload = notificationData;
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "AchievementType");
            data.Add("SubType", ((int)notificationData.Type).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            int badge = this.SprintParticipantRepo != null ? this.SprintParticipantRepo.GetParticipantUnreadNotificationCount(this.ParticipantUserId) : 0;
            data.Add("Count", badge.ToString());



            var message = new PushNotification.PushNotificationMulticastMessageBuilder(this.SprintParticipantRepo, this.ParticipantUserId)
                .Notification(notification.Title, notification.Body)
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