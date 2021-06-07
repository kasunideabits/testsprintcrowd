using System;
using System.Collections.Generic;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Common;
using SprintCrowd.BackEnd.Domain.Sprint.Dtos;

namespace SprintCrowd.BackEnd.Domain.Sprint
{
    public class SprintWithPariticpantsDto
    {
        public SprintWithPariticpantsDto(
            int sprintId,
            string sprintName,
            int distance,
            int numberOfParticipants,
            DateTime startTime,
            SprintType type,
            string sprintLocation,
            string promoCode,

            bool isTimeBased = false,
            TimeSpan durationForTimeBasedEvent = default(TimeSpan),
            string descriptionForTimeBasedEvent = null,
            bool InfluencerAvailability = false,
            bool isNarrationsOn = false,
            string coHost = "")
        {
            this.SprintInfo = new SprintInfoDto(sprintId, sprintName, distance, numberOfParticipants, startTime, type, sprintLocation, promoCode, null, isTimeBased, durationForTimeBasedEvent, descriptionForTimeBasedEvent, InfluencerAvailability, isNarrationsOn, coHost);

            this.ParticipantInfo = new List<ParticipantInfoDto>();
        }

        public SprintInfoDto SprintInfo { get; set; }

        public List<ParticipantInfoDto> ParticipantInfo { get; set; }

        public void AddParticipant(int id, string name, string profilePicture, string city, string country, string countryCode, string colorCode, bool creator, ParticipantStage stage, bool isInfluencer = false)
        {
            this.ParticipantInfo.Add(new ParticipantInfoDto(id, name, profilePicture, city, country, countryCode, colorCode, creator, stage, isInfluencer));
        }
    }

}