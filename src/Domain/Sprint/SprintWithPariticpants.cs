using System;
using System.Collections.Generic;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Common;

namespace SprintCrowd.BackEnd.Domain.Sprint
{
    public class SprintWithPariticpants
    {
        public SprintWithPariticpants(
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

        public void AddParticipant(int id, string name, string profilePicture, string city, string country, string countryCode, bool creator)
        {
            this.PariticipantInfo.Add(new ParticipantInfoDto(id, name, profilePicture, city, country, countryCode, creator));
        }
    }

    public class SprintInfoDto : SprintBaseDto
    {
        public SprintInfoDto(
            int id,
            string name,
            int distance,
            int numberOfParticipants,
            DateTime startTime,
            SprintType type,
            string location) : base(id, name, distance, numberOfParticipants, startTime, type)
        {
            this.Location = location;
            this.ExtendedTime = startTime.AddMinutes(15);
        }

        public string Location { get; }
        public DateTime ExtendedTime { get; }
    }

    public class ParticipantInfoDto : ParticipantBaseDto
    {
        public ParticipantInfoDto(
            int id,
            string name,
            string profilePicture,
            string city,
            string country,
            string countryCode,
            bool creator) : base(id, name, profilePicture, city, country, countryCode)
        {
            this.Creator = creator;
        }

        public bool Creator { get; }
    }

}