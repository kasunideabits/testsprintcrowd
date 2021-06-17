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
            bool isTimeBased = false,
            TimeSpan durationForTimeBasedEvent = default(TimeSpan),
            string descriptionForTimeBasedEvent = null,
            bool influencerAvailability = false,
            bool isNarrationsOn = true,
            string coHost = "",
            int interval = 15
            ) : base(id, name, distance, numberOfParticipants, startTime, type, imageUrl, promoCode, isTimeBased, durationForTimeBasedEvent, descriptionForTimeBasedEvent, influencerAvailability)

        {
            this.Location = location;
            this.IsNarrationsOn = isNarrationsOn;
            this.ExtendedTime = startTime.AddMinutes(interval);
            this.IsTimeBased = isTimeBased;
            this.DurationForTimeBasedEvent = durationForTimeBasedEvent;
            this.InfluencerAvailability = influencerAvailability;
            this.DescriptionForTimeBasedEvent = descriptionForTimeBasedEvent;
            this.CoHost = coHost;

        }

        public SprintInfoDto(
            Sprint sprint,
            string location = null//,
                                  //bool isTimeBased = false //,
                                  //TimeSpan durationForTimeBasedEvent = default(TimeSpan),
                                  //string descriptionForTimeBasedEvent = null,
                                  //bool InfluencerAvailability = false
            ) : base(sprint.Id, sprint.Name, sprint.Distance, sprint.NumberOfParticipants, sprint.StartDateTime, (SprintType)sprint.Type, sprint.ImageUrl, sprint.PromotionCode, sprint.IsTimeBased, sprint.DurationForTimeBasedEvent, sprint.DescriptionForTimeBasedEvent, sprint.InfluencerAvailability)
        {
            this.Location = location;
            this.ExtendedTime = sprint.StartDateTime.AddMinutes(sprint.Interval);
            this.IsSmartInvite = sprint.IsSmartInvite;
            this.SocialMediaLink = sprint.SocialMediaLink;
            this.IsTimeBased = sprint.IsTimeBased;
            this.DurationForTimeBasedEvent = sprint.DurationForTimeBasedEvent;
            this.InfluencerAvailability = sprint.InfluencerAvailability;

        }


        public string Location { get; }
        public bool IsSmartInvite { get; } = false;
        public string SocialMediaLink { get; set; } = string.Empty;
        public DateTime ExtendedTime { get; }
        public bool IsTimeBased { get; }
        public TimeSpan DurationForTimeBasedEvent { get; }
        public string DescriptionForTimeBasedEvent { get; }
        public bool InfluencerAvailability { get; }
        public bool IsNarrationsOn { get; set; } = true;

        public string CoHost { get; set; }
    }
}