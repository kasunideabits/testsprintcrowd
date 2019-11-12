using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    /// <summary>
    /// Factory which handle building notificaiton message dtos for different notification messages
    /// </summary>
    public static class NotificationDtoFactory
    {
        /// <summary>
        /// Build notification message
        /// </summary>
        /// <param name="sender">notification sender</param>
        /// <param name="receiver">notification receiver</param>
        /// <param name="notification">notification instance</param>
        /// <returns>sprint notificaiton</returns>
        public static ISprintNotification Build(User sender, User receiver, SprintNotification notification)
        {
            return SprintNotificationDtoFactory.Build(sender, receiver, notification);
        }
    }
}