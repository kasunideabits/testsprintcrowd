namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Jobs;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.Infrastructure.NotificationWorker.Reminders.Models;

    internal class SprintNotificationReminderJobs : ISprintNotificationReminderJobs
    {
        public SprintNotificationReminderJobs(ScrowdDbContext context)
        {
            this.Now = DateTime.UtcNow;
            this.Context = context;
        }

        private DateTime Now { get; }
        private ScrowdDbContext Context { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="distance"></param>
        /// <param name="startTime"></param>
        /// <param name="numberOfPariticipants"></param>
        /// <param name="sprintType"></param>
        /// <param name="sprintStatus"></param>
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
            this.Context.SaveChanges();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprintId"></param>
        public void RemoveTimeReminder(int sprintId)
        {
            var jobs = this.GetJobs(sprintId);
            foreach (var job in jobs)
            {
                new NotificationWorker<SprintTimeReminder>().DeleteSchedule(job.JobId);
            }
            this.RemoveHandler(jobs);
            this.Context.SaveChanges();
        }

        private void Execute(int id, string name, int distance, DateTime startTime, int numberOfPariticipants,
            SprintType sprintType, SprintStatus sprintStatus, SprintNotificaitonType notificationType, TimeSpan diff)
        {
            var message = new SprintReminderMessage(id, name, distance, startTime, numberOfPariticipants, sprintType, sprintStatus, notificationType);
            var token = new NotificationWorker<SprintTimeReminder>().Schedule(message, diff);
            this.AddJobHandler(id, token);
        }

        private void AddJobHandler(int sprintId, string token)
        {
            this.Context.ScheduleJobs.Add(new ScheduleJob() { TargetId = sprintId, JobId = token });
        }

        private IEnumerable<ScheduleJob> GetJobs(int sprintId)
        {
            var result = this.Context.ScheduleJobs.Where(s => s.TargetId == sprintId);
            return result;
        }

        private void RemoveHandler(IEnumerable<ScheduleJob> jobs)
        {
            this.Context.ScheduleJobs.RemoveRange(jobs);
        }

        private TimeSpan DayBefore(DateTime startTime) => startTime.AddHours(-24) - this.Now;

        private TimeSpan OneHourBefore(DateTime startTime) => startTime - startTime.AddHours(-1);

        private TimeSpan FifMBefore(DateTime startTime) => startTime.AddMinutes(-15) - this.Now;

        private TimeSpan OnLive(DateTime startTime) => startTime - this.Now;

        private TimeSpan FinalCall(DateTime startTime) => startTime.AddMinutes(8) - this.Now;

        private TimeSpan Expired(DateTime startTime) => startTime.AddHours(15) - this.Now;
    }
}