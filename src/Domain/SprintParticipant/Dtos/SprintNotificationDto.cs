namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    using System;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    public static class NotificationDto
    {
        public static NotificaitonPayload<SprintNotificationPayload> BuildNotification(SprintNotification notification)
        {
            var sprintNotificationDto = new SprintNotificationPayload(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.Sender,
                notification.Receiver);
            return new NotificaitonPayload<SprintNotificationPayload>()
            {
                Type = notification.Type,
                    CreateDate = notification.CreatedDate,
                    Data = sprintNotificationDto
            };
        }
    }

    public class NotificaitonPayload<T>
    {
        public SprintNotificaitonType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public T Data { get; set; }
    }

    public class SprintNotificationPayload
    {
        public SprintNotificationPayload(int sprintId, string sprintName, int distance, DateTime startDateTime, int numberOfParticipants, User inviter, User invitee)
        {
            this.Sprint = new SprintNotificationInfo(sprintId, sprintName, distance, startDateTime, numberOfParticipants);
            this.Inviter = new InvitationUser(inviter.Id, inviter.Name, inviter.Email, inviter.ProfilePicture);
            this.Invitee = new InvitationUser(invitee.Id, invitee.Name, invitee.Email, invitee.ProfilePicture);

        }
        public SprintNotificationInfo Sprint { get; }
        public InvitationUser Inviter { get; }
        public InvitationUser Invitee { get; }
    }

    public class SprintNotificationInfo
    {
        public SprintNotificationInfo(int id, string name, int distance, DateTime startTime, int numberOfParticipant)
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.StartTime = startTime;
            this.NumberOfParticipant = numberOfParticipant;
        }

        public int Id { get; }
        public string Name { get; }
        public int Distance { get; }
        public DateTime StartTime { get; }
        public int NumberOfParticipant { get; }
    }

    public class InvitationUser
    {
        public InvitationUser(int id, string name, string email, string profile)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Profile = profile;
        }
        public int Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Profile { get; }
    }
}