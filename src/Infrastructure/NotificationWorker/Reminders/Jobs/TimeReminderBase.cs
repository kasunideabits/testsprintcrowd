namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders
{
    using System.Collections.Generic;
    using System.IO;
    using System;
    using FirebaseAdmin.Messaging;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;

    internal class TimeReminderBase
    {
        public TimeReminderBase(ISprintParticipantRepo sprintParticipantRepo)
        {
            this.SprintParticipantRepo = sprintParticipantRepo;
            this.MessageBuilder = new PushNotificationMulticastMessageBuilder(null,0);
        }

        private ISprintParticipantRepo SprintParticipantRepo { get; }
        private PushNotificationMulticastMessageBuilder MessageBuilder { get; set; }

        public MulticastMessage BuildNotificationMessage(
            string userLang,
            string sprintName,
            int notificationId,
            SprintNotificaitonType notificationType,
            List<string> tokens,
            dynamic payload)
        {
            this.BuildNotification(userLang, notificationType, sprintName);
            this.BuildData(notificationId, notificationType, payload);
            return this.MessageBuilder.Tokens(tokens).Build();
        }

        private void BuildNotification(string userLang, SprintNotificaitonType notificationType, string sprintName)
        {
            var translation = this.GetTransaltion(userLang);
            JToken section = null;
            switch (notificationType)
            {
                case SprintNotificaitonType.TimeReminderBeforeStart:
                    section = translation["reminders"]["oneDayBefore"];
                    break;
                case SprintNotificaitonType.TimeReminderOneHourBefore:
                    section = translation["reminders"]["oneHourBeforeLive"];
                    break;
                case SprintNotificaitonType.TimeReminderBeforFiftyM:
                    section = translation["reminders"]["fifteenMBefore"];
                    break;
                case SprintNotificaitonType.TimeReminderStarted:
                    section = translation["reminders"]["onLive"];
                    break;
                case SprintNotificaitonType.TimeReminderFinalCall:
                    section = translation["reminders"]["finalCall"];
                    break;
                case SprintNotificaitonType.TimeReminderExpired:
                    section = translation["reminders"]["expired"];
                    break;
            }
            SCFireBaseNotificationMessage message = new SCFireBaseNotificationMessage(section);
            this.MessageBuilder.Notification(message.Title, String.Format(message.Body, sprintName));
        }

        private void BuildData(int notificationId, SprintNotificaitonType notificationType, dynamic payload)
        {
            var data = new Dictionary<string, string>();
            data.Add("NotificationId", notificationId.ToString());
            data.Add("MainType", "SprintType");
            data.Add("SubType", ((int)notificationType).ToString());
            data.Add("CreateDate", DateTime.UtcNow.ToString());
            data.Add("Data", JsonConvert.SerializeObject(payload));
            this.MessageBuilder.Message(data);
        }

        private JToken GetTransaltion(string userLang)
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
    }
}