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
            string sprintLocation)
        {
            this.SprintInfo = new SprintInfoDto(sprintId, sprintName, distance, numberOfParticipants, startTime, type, sprintLocation);
            this.PariticipantInfo = new List<ParticipantInfoDto>();
        }

        public SprintInfoDto SprintInfo { get; set; }

        public List<ParticipantInfoDto> PariticipantInfo { get; set; }

        public void AddParticipant(int id, string name, string profilePicture, string city, string country, string countryCode, string colorCode, bool creator, ParticipantStage stage)
        {
            this.PariticipantInfo.Add(new ParticipantInfoDto(id, name, profilePicture, city, country, countryCode, colorCode, creator, stage));
        }
    }

}