namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class SprintInfoDto : SprintBaseDto
    {
        public SprintInfoDto(
            int id,
            string name,
            int distance,
            int numberOfParticipants,
            DateTime startTime,
            SprintType type,
            string imageUrl,
            string promoCode,
            string location = null,
            bool isNarrationsOn = true) : base(id, name, distance, numberOfParticipants, startTime, type, imageUrl, promoCode)
        {
            this.Location = location;
            this.IsNarrationsOn = isNarrationsOn;
            this.ExtendedTime = startTime.AddMinutes(15);
        }

        public SprintInfoDto(
            Sprint sprint,
            string location = null
            ) : base(sprint.Id, sprint.Name, sprint.Distance, sprint.NumberOfParticipants, sprint.StartDateTime, (SprintType)sprint.Type, sprint.ImageUrl, sprint.PromotionCode)
        {
            this.Location = location;
            this.ExtendedTime = sprint.StartDateTime.AddMinutes(15);
            this.IsSmartInvite = sprint.IsSmartInvite;
            this.SocialMediaLink = sprint.SocialMediaLink;
        }


        public string Location { get; }
        public bool IsSmartInvite { get; } = false;
        public string SocialMediaLink { get; set; } = string.Empty;
        public DateTime ExtendedTime { get; }

        public bool IsNarrationsOn { get; set; } = true;
    }
}