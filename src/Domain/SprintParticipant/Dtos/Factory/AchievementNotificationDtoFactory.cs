using System;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    public static class AchievementNotificationDtoFactory
    {
        public static IAchievementNotification Build(AchievementNoticiation notification)
        {
            return new AchievementDto(notification);
        }

        internal class AchievementBaseDto
        {
            public AchievementBaseDto(AchievementNoticiation notification)
            {
                this.MainType = "AchievementType";
                this.SubType = (int)notification.AchievementType;
                this.NotificationId = notification.Id;
                this.CreateDate = notification.CreatedDate;
            }

            public string MainType { get; }
            public int SubType { get; }
            public int NotificationId { get; }
            public DateTime CreateDate { get; }
        }

        internal class AchievementDto : AchievementBaseDto, IAchievementNotification
        {
            public AchievementDto(AchievementNoticiation notification) : base(notification)
            {
                this.Data = new AchievementPayload(notification.AchievementType, notification.AchievedOn);
            }
            public AchievementPayload Data { get; }
        }

        internal class AchievementPayload
        {
            public AchievementPayload(AchievementType type, DateTime achievedOn)
            {
                this.Type = type;
                this.AchievedOn = achievedOn;
            }

            public AchievementType Type { get; }
            public DateTime AchievedOn { get; }
        }
    }
}