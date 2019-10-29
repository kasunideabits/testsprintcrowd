using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    public static class NotificationDtoFactory
    {
        public static ISprintNotification Build(SprintNotification notification)
        {
            return SprintNotificationDtoFactory.Build(notification);
        }
    }
}