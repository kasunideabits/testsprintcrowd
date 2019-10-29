using System;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    public static class SprintNotificationDtoFactory
    {
        public static ISprintNotification Build(SprintNotification notification)
        {
            switch (notification.Type)
            {
                case SprintNotificaitonType notificaitonType when
                notificaitonType == SprintNotificaitonType.InvitationRequest ||
                notificaitonType == SprintNotificaitonType.InvitationAccept ||
                notificaitonType == SprintNotificaitonType.InvitationDecline:
                    return new SprintInvitationDto(notification);
                
            }
            throw new ApplicationException();
        }
    }

    internal class SprintInvitationDto : ISprintNotification
    {
        public SprintInvitationDto(SprintNotification notification)
        {
            this.Type = notification.Type;
            this.CreateDate = notification.CreatedDate;
            this.Data = new SprintNotificationPayload(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.Sender,
                notification.Receiver
            );
        }

        public SprintNotificaitonType Type { get; }
        public DateTime CreateDate { get; }
        public dynamic Data { get; }
    }

    internal class SprintNotificationPayload
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

    internal class SprintNotificationInfo
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

    internal class InvitationUser
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