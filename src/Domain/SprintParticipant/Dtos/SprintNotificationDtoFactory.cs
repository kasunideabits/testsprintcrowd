using System;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    public static class SprintNotificationDtoFactory
    {
        public static ISprintNotification Build(SprintNotification notification)
        {
            switch (notification.SprintNotificationType)
            {
                case SprintNotificaitonType notificaitonType when
                notificaitonType == SprintNotificaitonType.InvitationRequest:
                    return new SprintInvitationRequestDto(notification);
                case SprintNotificaitonType notificaitonType when
                notificaitonType == SprintNotificaitonType.InvitationAccept ||
                notificaitonType == SprintNotificaitonType.InvitationDecline:
                    return new SprintInvitationResponseDto(notification);
            }
            throw new Application.ApplicationException();
        }
    }

    internal class SprintInvitationRequestDto : ISprintNotification
    {
        public SprintInvitationRequestDto(SprintNotification notification)
        {
            this.MainType = "SprintType";
            this.NotificationId = notification.Id;
            this.SubType = notification.SprintNotificationType;
            this.CreateDate = notification.CreatedDate;
            this.Data = new SprintNotificationPayload(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.SprintType,
                notification.SprintStatus,
                notification.Sender,
                notification.Receiver
            );
        }

        public string MainType { get; }
        public int NotificationId { get; }
        public SprintNotificaitonType SubType { get; }
        public DateTime CreateDate { get; }
        public dynamic Data { get; }
    }

    internal class SprintInvitationResponseDto : ISprintNotification
    {
        public SprintInvitationResponseDto(SprintNotification notification)
        {
            this.MainType = "SprintType";
            this.NotificationId = notification.Id;
            this.SubType = notification.SprintNotificationType;
            this.CreateDate = notification.CreatedDate;
            this.Data = new SprintInvitationResponsePayload(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.SprintType,
                notification.SprintStatus,
                notification.Sender
            );
        }

        public string MainType { get; }
        public int NotificationId { get; }
        public SprintNotificaitonType SubType { get; }
        public DateTime CreateDate { get; }
        public dynamic Data { get; }
    }

    internal class SprintInvitationResponsePayload
    {
        public SprintInvitationResponsePayload(
            int sprintId,
            string sprintName,
            int distance,
            DateTime startDateTime,
            int numberOfParticipants,
            SprintType sprintType,
            SprintStatus sprintStatus,
            User user)
        {
            this.Sprint = new SprintNotificationInfo(sprintId, sprintName, distance, startDateTime, numberOfParticipants, sprintType, sprintStatus);
            this.User = new InvitationUser(user.Id, user.Name, user.Email, user.ProfilePicture, user.Code, user.City, user.Country, user.CountryCode);
        }
        public SprintNotificationInfo Sprint { get; }
        public InvitationUser User { get; }
    }

    internal class SprintNotificationPayload
    {
        public SprintNotificationPayload(
            int sprintId,
            string sprintName,
            int distance,
            DateTime startDateTime,
            int numberOfParticipants,
            SprintType sprintType,
            SprintStatus sprintStatus,
            User inviter,
            User invitee)
        {
            this.Sprint = new SprintNotificationInfo(sprintId, sprintName, distance, startDateTime, numberOfParticipants, sprintType, sprintStatus);
            this.Inviter = new InvitationUser(inviter.Id, inviter.Name, inviter.Email, inviter.ProfilePicture, inviter.Code, inviter.City, inviter.Country, inviter.CountryCode);
            this.Invitee = new InvitationUser(invitee.Id, invitee.Name, invitee.Email, invitee.ProfilePicture, invitee.Code, invitee.City, invitee.Country, invitee.CountryCode);

        }
        public SprintNotificationInfo Sprint { get; }
        public InvitationUser Inviter { get; }
        public InvitationUser Invitee { get; }
    }

    internal class SprintNotificationInfo
    {
        public SprintNotificationInfo(
            int id,
            string name,
            int distance,
            DateTime startTime,
            int numberOfParticipant,
            SprintType sprintType,
            SprintStatus sprintStatus)
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.StartTime = startTime;
            this.NumberOfParticipant = numberOfParticipant;
            this.SprintStatus = sprintStatus;
            this.SprintType = sprintType;
        }

        public int Id { get; }
        public string Name { get; }
        public int Distance { get; }
        public DateTime StartTime { get; }
        public int NumberOfParticipant { get; }
        public SprintStatus SprintStatus { get; }
        public SprintType SprintType { get; }

    }

    internal class InvitationUser
    {
        public InvitationUser(int id, string name, string email, string profile, string code, string city, string country, string countryCode)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Profile = profile;
            this.Code = code;
            this.City = city;
            this.Country = country;
            this.CountryCode = countryCode;
        }
        public int Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Profile { get; }
        public string Code { get; }
        public string City { get; }
        public string Country { get; }
        public string CountryCode { get; }
    }

}