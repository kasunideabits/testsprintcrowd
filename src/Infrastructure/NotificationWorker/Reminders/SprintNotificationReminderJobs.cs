namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders
{
    using System;
    using SprintCrowd.BackEnd.Application;
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

        public void TimeReminder(int id, string name, int distance, DateTime startTime, int numberOfPariticipants,
            SprintType sprintType, SprintStatus sprintStatus)
        {
            var diff = startTime - this.Now;
            // 24 Hour     =========>
            if (startTime > this.Now && diff.TotalHours >= 24)
            {
                this.Execute(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus, SprintNotificaitonType.TimeReminderBeforeStart, this.DayBefore(startTime));
            }

            // One Hour    =========>
            if (diff.TotalHours >= 1)
            {
                this.Execute(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus, SprintNotificaitonType.TimeReminderOneHourBefore, this.OneHourBefore(startTime));
            }

            if (startTime > this.Now)
            {
                // 15 Minutes  =========>
                this.Execute(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus, SprintNotificaitonType.TimeReminderBeforFiftyM, this.FifMBefore(startTime));
                // Live        =========>
                this.Execute(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus, SprintNotificaitonType.TimeReminderStarted, this.OnLive(startTime));
                // Final Call  =========>
                this.Execute(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus, SprintNotificaitonType.TimeReminderFinalCall, this.FinalCall(startTime));
                // Expired     =========>
                this.Execute(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus, SprintNotificaitonType.TimeReminderExpired, this.Expired(startTime));
            }
        }

        private void Execute(int id, string name, int distance, DateTime startTime, int numberOfPariticipants,
            SprintType sprintType, SprintStatus sprintStatus, SprintNotificaitonType notificationType, TimeSpan diff)
        {
            var message = new SprintReminderMessage(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus, notificationType);
            new NotificationWorker<SprintTimeReminder>().Schedule(message, diff);
        }

        private TimeSpan DayBefore(DateTime startTime) => startTime.AddHours(-24) - this.Now;

        private TimeSpan OneHourBefore(DateTime startTime) => startTime - startTime.AddHours(-1);

        private TimeSpan FifMBefore(DateTime startTime) => startTime.AddMinutes(-15) - this.Now;

        private TimeSpan OnLive(DateTime startTime) => startTime - this.Now;

        private TimeSpan FinalCall(DateTime startTime) => startTime.AddMinutes(8) - this.Now;

        private TimeSpan Expired(DateTime startTime) => startTime.AddHours(15) - this.Now;
    }
}