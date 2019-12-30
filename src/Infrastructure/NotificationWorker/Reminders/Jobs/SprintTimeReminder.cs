namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Jobs
{
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Repo;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;
    using SprintCrowd.Infrastructure.NotificationWorker.Reminders.Models;

    internal class SprintTimeReminder : TimeReminderBase, ISprintTimeReminder
    {

        public SprintTimeReminder(ScrowdDbContext context, IPushNotificationClient client)
        {
            this.SprintReminderRepo = new SprintReminderRepo(context);
            this.Client = client;
        }

        private ISprintReminderRepo SprintReminderRepo { get; }
        private IPushNotificationClient Client { get; }

        public void Run(object message = null)
        {
            SprintReminderMessage sprintReminder = message as SprintReminderMessage;
            if (sprintReminder != null)
            {
                // var payload = SprintReminderDtoMapper.Map(sprintReminder);
                var sprint = this.SprintReminderRepo.GetSprint(sprintReminder.Sprint.Id);
                var systemUser = this.SprintReminderRepo.GetSystemUser();
                var participants = this.SprintReminderRepo.GetParticipantIdsByLangugage(sprintReminder.Sprint.Id, sprintReminder.NotificationType);
                foreach (var lang in participants)
                {
                    var notificationId = this.SprintReminderRepo.AddNotification(sprintReminder.NotificationType, sprint.Id, sprint.Name, sprint.Distance,
                        (SprintType)sprint.Type, (SprintStatus)sprint.Status, sprint.NumberOfParticipants, sprint.StartDateTime, systemUser.Id);
                    this.SprintReminderRepo.AddUserNotification(lang.Value, systemUser.Id, notificationId);
                    var tokens = this.SprintReminderRepo.GetTokens(lang.Value);
                    var notificaiton = this.BuildNotificationMessage(lang.Key, sprintReminder.Sprint.Name, systemUser.Id, sprintReminder.NotificationType, tokens, sprintReminder);
                    this.Client.SendMulticaseMessage(notificaiton);
                }
                this.SprintReminderRepo.SaveChanges();
            }
        }

        // public static class SprintReminderDtoMapper
        // {
        //     public static SprintReminderMessageDto Map(SprintReminderMessage message)
        //     {
        //         return new SprintReminderMessageDto(message.Sprint, message.NotificationType);
        //     }
        // }
    }
}