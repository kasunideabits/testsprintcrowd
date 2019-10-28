using System;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Common;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    public class SprintParticipantDto
    {
        public SprintParticipantDto(
            int sprintId,
            string sprintName,
            int distance,
            int numberOfParticipants,
            DateTime startTime,
            SprintType sprintType,
            int userId,
            string userName,
            string profilePicture,
            string city,
            string country,
            string countryCode)
        {
            this.SprintInfo = new SprintInfoDto(sprintId, sprintName, distance, numberOfParticipants, startTime, sprintType);
            this.ParticipantInfo = new ParticipantInfoDto(userId, userName, profilePicture, city, country, countryCode);
        }

        public SprintInfoDto SprintInfo { get; }
        public ParticipantInfoDto ParticipantInfo { get; }

    }

    public class SprintInfoDto : SprintBaseDto
    {
        public SprintInfoDto(
            int id,
            string name,
            int distance,
            int numberOfParticipants,
            DateTime startTime,
            SprintType type) : base(id, name, distance, numberOfParticipants, startTime, type) { }
    }

    public class ParticipantInfoDto : ParticipantBaseDto
    {
        public ParticipantInfoDto(
            int id,
            string name,
            string profilePicture,
            string city,
            string country,
            string countryCode) : base(id, name, profilePicture, city, country, countryCode) { }
    }
}