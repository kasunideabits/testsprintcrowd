namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Jobs
{
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Dtos;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Reminders.Repo;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;
    using SprintCrowd.Infrastructure.NotificationWorker.Reminders.Models;

    internal class SprintTimeReminder : TimeReminderBase, ISprintTimeReminder
    {

        public SprintTimeReminder(ScrowdDbContext context, IPushNotificationClient client, ISprintParticipantRepo sprintParticipantRepo) : base()
        {
            this.SprintReminderRepo = new SprintReminderRepo(context);
            this.Client = client;
            this.SprintParticipantRepo = sprintParticipantRepo;
        }
        private ISprintParticipantRepo SprintParticipantRepo { get; }
        private ISprintReminderRepo SprintReminderRepo { get; }
        private IPushNotificationClient Client { get; }

        public void Run(object message = null)
        {
            SprintReminderMessage sprintReminder = null;
            if (message != null)
                sprintReminder = message as SprintReminderMessage;
            if (sprintReminder != null)
            {
                // var payload = SprintReminderDtoMapper.Map(sprintReminder);
                var sprint = this.SprintReminderRepo.GetSprint(sprintReminder.Sprint.Id);

                if (sprint != null)
                    if (sprint.Status != (int)SprintStatus.ARCHIVED)
                    {
                        var systemUser = this.SprintReminderRepo.GetSystemUser();
                        var participants = this.SprintReminderRepo.GetParticipantIdsByLangugage(sprintReminder.Sprint.Id, sprintReminder.NotificationType);
                        foreach (var lang in participants)
                        {
                            var notificationId = this.SprintReminderRepo.AddNotification(sprintReminder.NotificationType, sprint.Id, sprint.Name, sprint.Distance,
                                (SprintType)sprint.Type, (SprintStatus)sprint.Status, sprint.NumberOfParticipants, sprint.StartDateTime, systemUser.Id);
                            this.SprintReminderRepo.AddUserNotification(lang.Value, systemUser.Id, notificationId);
                            //var tokens = this.SprintReminderRepo.GetTokens(lang.Value);

                            lang.Value.ForEach(receiverId =>
                            {
                                var token = this.SprintReminderRepo.GetToken(receiverId);
                                var messageBuilder = new PushNotification.PushNotificationMulticastMessageBuilder(this.SprintParticipantRepo, receiverId);
                                var notificaiton = this.BuildNotificationMessage(lang.Key, sprint.Name, systemUser.Id, sprintReminder.NotificationType, token, sprintReminder, receiverId, messageBuilder, this.SprintParticipantRepo);
                            //this.Client.SendMulticaseMessage(notificaiton);
                            this.Client.SendMulticaseMessage(notificaiton);
                            });
                        }
                        this.SprintReminderRepo.SaveChanges();
                    }
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