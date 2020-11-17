namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System.Collections.Generic;
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Domain.Sprint.Dtos;

    public class CreateSprintDto
    {
        public CreateSprintDto(
            int sprintId,
            string sprintName,
            int distance,
            int numberOfParticipants,
            DateTime startTime,
            SprintType type,
            int userId,
            string userName,
            string profilePicture,
            string city,
            string country,
            string countryCode,
            string colorCode,
            bool creator,
            ParticipantStage stage
        )
        {
            this.SprintInfo = new SprintInfoDto(sprintId, sprintName, distance, numberOfParticipants, startTime, type, string.Empty ,string.Empty);
            var participant = new List<ParticipantInfoDto>();
            participant.Add(new ParticipantInfoDto(userId, userName, profilePicture, city, country, countryCode, colorCode, creator, stage));
            this.ParticipantInfo = participant;
        }

        public SprintInfoDto SprintInfo { get; }
        public List<ParticipantInfoDto> ParticipantInfo { get; }
    }
}