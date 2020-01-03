namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders
{
    using System;
    using SprintCrowd.BackEnd.Application;

    public interface ISprintNotificationReminderJobs
    {
        void TimeReminder(int id, string name, int distance, DateTime startTime,
            int numberOfPariticipants, SprintType sprintType, SprintStatus sprintStatus);
    }

}