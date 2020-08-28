using System;
using Newtonsoft.Json;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    /// <summary>
    /// Factory which build sprint notificaiton message types dtos
    /// </summary>
    public static class SprintNotificationDtoFactory
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
            switch (notification.SprintNotificationType)
            {
                case SprintNotificaitonType notificaitonType when
                notificaitonType == SprintNotificaitonType.InvitationRequest:
                    return new SprintInvitationRequestDto(sender, receiver, notification);
                case SprintNotificaitonType notificaitonType when
                notificaitonType == SprintNotificaitonType.InvitationAccept ||
                notificaitonType == SprintNotificaitonType.InvitationDecline ||
                notificaitonType == SprintNotificaitonType.FriendRequestAccept ||
                notificaitonType == SprintNotificaitonType.LeaveParticipant:
                    return new SprintInvitationResponseDto(sender, notification);
                case SprintNotificaitonType notificaitonType when
                notificaitonType == SprintNotificaitonType.Remove ||
                notificaitonType == SprintNotificaitonType.RemoveParticipsnt:
                    return new SprintRemoveResponseDto(sender, notification);
                case SprintNotificaitonType notificaitonType when
                notificaitonType == SprintNotificaitonType.Edit:
                    return new SprintEditResponseDto(sender, notification);
                case SprintNotificaitonType notificaitonType when
                notificaitonType == SprintNotificaitonType.TimeReminderBeforeStart ||
                notificaitonType == SprintNotificaitonType.TimeReminderOneHourBefore ||
                notificaitonType == SprintNotificaitonType.TimeReminderBeforFiftyM ||
                notificaitonType == SprintNotificaitonType.TimeReminderStarted ||
                notificaitonType == SprintNotificaitonType.TimeReminderFinalCall ||
                notificaitonType == SprintNotificaitonType.TimeReminderExpired:
                    return new SprintTimeReminderDto(notification);
                case SprintNotificaitonType notificaitonType when notificaitonType == SprintNotificaitonType.FriendJoin:
                    return new SprintInvitationResponseDto(sender, notification);
                default:
                    break;
            }
            throw new Application.ApplicationException();
        }
    }

    internal class SprintNotificationBaseDto
    {
        public SprintNotificationBaseDto(SprintNotification notification)
        {
            this.MainType = "SprintType";
            this.NotificationId = notification.Id;
            this.SubType = notification.SprintNotificationType;
            this.CreateDate = notification.CreatedDate;
        }

        public string MainType { get; }
        public int NotificationId { get; }
        public SprintNotificaitonType SubType { get; }
        public DateTime CreateDate { get; }
    }

    internal class SprintInvitationRequestDto : SprintNotificationBaseDto, ISprintNotification
    {
        public SprintInvitationRequestDto(User sender, User receiver, SprintNotification notification) : base(notification)
        {
            this.Data = new SprintNotificationPayload(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.SprintType,
                notification.SprintStatus,
                sender,
                receiver
            );
        }
        public SprintNotificationPayload Data { get; }
    }

    internal class SprintInvitationResponseDto : SprintNotificationBaseDto, ISprintNotification
    {
        public SprintInvitationResponseDto(User sender, SprintNotification notification) : base(notification)
        {
            this.Data = new SprintInvitationResponsePayload(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.SprintType,
                notification.SprintStatus,
                sender
            );
        }
        public SprintInvitationResponsePayload Data { get; }
    }

    internal class SprintRemoveResponseDto : SprintNotificationBaseDto, ISprintNotification
    {
        public SprintRemoveResponseDto(User sender, SprintNotification notification) : base(notification)
        {
            this.Data = new SprintRemoveResponsePayload(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.SprintStatus,
                notification.SprintType,
                sender.Id,
                sender.Name,
                sender.ProfilePicture,
                sender.Code,
                sender.ColorCode,
                sender.City,
                sender.Country,
                sender.CountryCode
            );
        }
        public SprintRemoveResponsePayload Data { get; }
    }

    internal class SprintEditResponseDto : SprintNotificationBaseDto, ISprintNotification
    {
        public SprintEditResponseDto(User editor, SprintNotification notification) : base(notification)
        {
            this.Data = new SprintEditResponsePayload(editor, notification);
        }
        public SprintEditResponsePayload Data { get; }
    }

    internal class SprintTimeReminderDto : SprintNotificationBaseDto, ISprintNotification
    {
        public SprintTimeReminderDto(SprintNotification notification) : base(notification)
        {
            this.Data = new SprintTimeReminderResponsePayload(notification, notification.SprintNotificationType);
        }
        public SprintTimeReminderResponsePayload Data { get; }
    }

    internal class SprintTimeReminderResponsePayload
    {
        public SprintTimeReminderResponsePayload(SprintNotification notification, SprintNotificaitonType notificationType)
        {
            this.Sprint = new SprintNotificationInfo(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.SprintType,
                notification.SprintStatus
            );
            this.NotificationType = notificationType;
        }

        public SprintNotificationInfo Sprint { get; }

        /// <summary>
        /// notification reminder time indication
        /// </summary>
        public SprintNotificaitonType NotificationType { get; }
    }

    internal class SprintEditResponsePayload
    {
        public SprintEditResponsePayload(User editer, SprintNotification notification)
        {
            this.Sprint = new SprintNotificationInfo(
                notification.SprintId,
                notification.SprintName,
                notification.Distance,
                notification.StartDateTime,
                notification.NumberOfParticipants,
                notification.SprintType,
                notification.SprintStatus
            );
            this.EditedBy = new NotificationUserInfo(
                editer.Id,
                editer.Name,
                editer.Email,
                editer.ProfilePicture,
                editer.Code,
                editer.City,
                editer.Country,
                editer.CountryCode
            );
        }
        public SprintNotificationInfo Sprint { get; }
        public NotificationUserInfo EditedBy { get; }
    }

    internal class SprintRemoveResponsePayload
    {
        public SprintRemoveResponsePayload(
            int sprintId,
            string sprintName,
            int distance,
            DateTime startTime,
            int numberOfParticipant,
            SprintStatus sprintStatus,
            SprintType sprintType,
            int userId,
            string name,
            string profilePicture,
            string code,
            string colorCode,
            string city,
            string country,
            string countryCode)
        {
            this.Sprint = new SprintNotificationInfo(sprintId, sprintName, distance, startTime, numberOfParticipant, sprintType, sprintStatus);
            this.DeletedBy = new NotificationUserInfo(userId, name, profilePicture, code, colorCode, city, country, countryCode);
        }
        public SprintNotificationInfo Sprint { get; }
        public NotificationUserInfo DeletedBy { get; }
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
            this.User = new NotificationUserInfo(user.Id, user.Name, user.Email, user.ProfilePicture, user.Code, user.City, user.Country, user.CountryCode);
        }
        public SprintNotificationInfo Sprint { get; }
        public NotificationUserInfo User { get; }
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
            this.Inviter = new NotificationUserInfo(inviter.Id, inviter.Name, inviter.Email, inviter.ProfilePicture, inviter.Code, inviter.City, inviter.Country, inviter.CountryCode);
            this.Invitee = new NotificationUserInfo(invitee.Id, invitee.Name, invitee.Email, invitee.ProfilePicture, invitee.Code, invitee.City, invitee.Country, invitee.CountryCode);

        }
        public SprintNotificationInfo Sprint { get; }
        public NotificationUserInfo Inviter { get; }
        public NotificationUserInfo Invitee { get; }
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

    /// <summary>
    /// Info class for handle notifiation
    /// </summary>
    internal class NotificationUserInfo
    {
        public NotificationUserInfo(int id, string name, string email, string profile, string code, string city, string country, string countryCode)
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