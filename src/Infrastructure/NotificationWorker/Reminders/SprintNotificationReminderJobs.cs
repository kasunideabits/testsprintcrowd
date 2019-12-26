namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Jobs;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.Infrastructure.NotificationWorker.Reminders.Models;

    internal class SprintNotificationReminderJobs : ISprintNotificationReminderJobs
    {
        public void TimeReminder(int sprintId, string sprintName, DateTime startTime)
        {
            var now = DateTime.UtcNow;
            var diff = startTime - now;

            // 24 Hour     =========>
            if (startTime > now && diff.TotalHours >= 24)
            {
                TimeSpan delay = startTime.AddHours(-24) - now;
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderBeforeStart, diff);
            }

            // One Hour    =========>
            if (diff.TotalHours >= 1)
            {
                TimeSpan delay = startTime - startTime.AddHours(-1);
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderOneHourBefore, diff);
            }

            // 10 Minutes  =========>
            if (diff.Minutes >= 15 && diff.TotalHours < 1)
            {
                TimeSpan delay = startTime.AddMinutes(-15) - now;
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderBeforFiftyM, diff);
            }

            // Live        =========>
            if (startTime > now)
            {
                TimeSpan delay = startTime - now;
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderStarted, diff);
            }

            // Final Call  =========>
            if (startTime < now && startTime > startTime.AddMinutes(-15))
            {
                TimeSpan delay = startTime.AddMinutes(8) - now;
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderFinalCall, diff);
            }

            // Expired     =========>
            if (startTime > now && diff.TotalHours < 0)
            {
                TimeSpan delay = startTime.AddHours(20) - now;
                this.Execute(sprintId, sprintName, SprintNotificaitonType.TimeReminderExpired, diff);
            }
        }

        private void Execute(int sprintId, string sprintName, SprintNotificaitonType notificaitonType, TimeSpan diff)
        {
            var message = new SprintReminderMessage(sprintId, sprintName, notificaitonType);
            new NotificationWorker<SprintTimeReminder>().Schedule(message, diff);
        }
    }

}