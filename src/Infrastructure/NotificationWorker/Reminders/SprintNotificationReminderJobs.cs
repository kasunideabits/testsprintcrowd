namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Jobs;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.Infrastructure.NotificationWorker.Reminders.Models;

    internal class SprintNotificationReminderJobs : ISprintNotificationReminderJobs
    {
        public SprintNotificationReminderJobs()
        {
            this.Now = DateTime.UtcNow;
        }

        private DateTime Now { get; }

        public void TimeReminder(int sprintId, string sprintName, DateTime startTime)
        {
            var diff = startTime - this.Now;
            // 24 Hour     =========>
            if (startTime > this.Now && diff.TotalHours >= 24)
            {
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderBeforeStart, this.DayBefore(startTime));
            }

            // One Hour    =========>
            if (diff.TotalHours >= 1)
            {
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderOneHourBefore, this.OneHourBefore(startTime));
            }

            if (startTime > this.Now)
            {
                // 15 Minutes  =========>
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderBeforFiftyM, this.FifMBefore(startTime));
                // Live        =========>
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderStarted, this.OnLive(startTime));
                // Final Call  =========>
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderFinalCall, this.FinalCall(startTime));
                // Expired     =========>
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderExpired, this.Expired(startTime));
            }
        }

        private void Execute(int sprintId, string sprintName, SprintNotificaitonType notificaitonType, TimeSpan diff)
        {
            var message = new SprintReminderMessage(sprintId, sprintName, notificaitonType);
            new NotificationWorker<SprintTimeReminder>().Schedule(message, diff);
        }

        private TimeSpan DayBefore(DateTime startTime) => startTime.AddHours(-24) - this.Now;

        private TimeSpan OneHourBefore(DateTime startTime) => startTime - startTime.AddHours(-1);

        private TimeSpan FifMBefore(DateTime startTime) => startTime.AddMinutes(-15) - this.Now;

        private TimeSpan OnLive(DateTime startTime) => startTime - this.Now;

        private TimeSpan FinalCall(DateTime startTime) => startTime.AddMinutes(8) - this.Now;

        private TimeSpan Expired(DateTime startTime) => startTime.AddHours(20) - this.Now;
    }
}