namespace SprintCrowd.Infrastructure.NotificationWorker.Reminders
{
    using System.Collections.Generic;
    using System;
    using Newtonsoft.Json;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;

    internal class TimeReminderBase
    {
        public TimeReminderBase()
        {
            this.MessageBuilder = new PushNotificationMulticastMessageBuilder();
        }

        private PushNotificationMulticastMessageBuilder MessageBuilder { get; set; }

        private dynamic BuildNotificationMessage(
            string userLang,
            string sprintName,
            int notificationId,
            SprintNotificaitonType notificationType,
            List<string> tokens,
            dynamic payload)
        {
            this.BuildNotification(userLang, notificationType, sprintName);
            this.BuildData(notificationId, notificationType, payload);
            this.MessageBuilder.Tokens(tokens).Build();
            return this.MessageBuilder;
        }

        private void BuildNotification(string userLang, SprintNotificaitonType notificationType, string sprintName)
        {
            var notification = this.GetNotificationMessage(userLang, sprintName);
            switch (notificationType)
            {
                case SprintNotificaitonType.TimeReminderBeforeStart:
                    notification.OndDayBefore();
                    break;
                case SprintNotificaitonType.TimeReminderOneHourBefore:
                    notification.OneHourBeforeLive();
                    break;
                case SprintNotificaitonType.TimeReminderBeforFiftyM:
                    notification.OneHourBeforeLive();
                    break;
                case SprintNotificaitonType.TimeReminderStarted:
                    notification.OnLive();
                    break;
                case SprintNotificaitonType.TimeReminderFinalCall:
                    notification.FinalCall();
                    break;
                case SprintNotificaitonType.TimeReminderExpired:
                    notification.FinalCall();
                    break;
            }
            this.MessageBuilder.Notification(notification.GetTitle(), String.Empty);
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

        private INotificationMessage GetNotificationMessage(string userLang, string sprintName)
        {
            switch (userLang)
            {
                case "en":
                    return new NotificationMessageEn(sprintName);
                case "se":
                    return new NotificationMessageSE(sprintName);
                default:
                    return new NotificationMessageEn(sprintName);
            }
        }
    }

    internal interface INotificationMessage
    {
        void FinalCall();
        void OndDayBefore();
        void OneHourBeforeLive();
        void OnLive();
        void Expired();
        string GetTitle();
        string GetBody();
    }

    internal class NotificationMessageEn : INotificationMessage
    {
        private string Title { get; set; }
        public string Body { get; set; }
        private string sprintName { get; }

        public NotificationMessageEn(string sprintName)
        {
            this.sprintName = sprintName;
        }

        public void FinalCall()
        {
            this.Title = $"Final call for {this.sprintName}";
        }

        public void OndDayBefore()
        {
            this.Title = $"24 hour before {this.sprintName} goes Live";
        }

        public void OneHourBeforeLive()
        {
            this.Title = $"1 hour before {this.sprintName} goes Live";
        }

        public void OnLive()
        {
            this.Title = $"{this.sprintName} is now Live";
        }

        public void Expired()
        {
            this.Title = $"Tou failed to mark attendace for {sprintName}";
        }

        public string GetTitle() => this.Title;

        public string GetBody() => this.Body;

    }

    internal class NotificationMessageSE : INotificationMessage
    {
        private string Title { get; set; }
        private string Body { get; set; }
        private string sprintName { get; }

        public NotificationMessageSE(string sprintName)
        {
            this.sprintName = sprintName;
        }

        public void FinalCall()
        {
            this.Title = $"Final call for {this.sprintName}";
        }

        public void OndDayBefore()
        {
            this.Title = $"24 hour before {this.sprintName} goes Live";
        }

        public void OneHourBeforeLive()
        {
            this.Title = $"1 hour before {this.sprintName} goes Live";
        }

        public void OnLive()
        {
            this.Title = $"{this.sprintName} is now Live";
        }

        public void Expired()
        {
            this.Title = $"Tou failed to mark attendace for {sprintName}";
        }

        public string GetTitle() => this.Title;

        public string GetBody() => this.Body;
    }

}