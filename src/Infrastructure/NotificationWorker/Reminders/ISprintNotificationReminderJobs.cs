using System;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders
{
    public interface ISprintNotificationReminderJobs
    {
        void TimeReminder(int sprintId, string sprintName, DateTime startTime);
    }

}