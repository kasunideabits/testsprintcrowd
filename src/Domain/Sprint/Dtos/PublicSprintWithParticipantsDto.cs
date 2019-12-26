namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    using System.Collections.Generic;
    using System;
    using SprintCrowd.BackEnd.Application;

    public class PublicSprintWithParticipantsDto
    {
        public PublicSprintWithParticipantsDto(int sprintId, string sprintName, int distance,
            int numberOfParticipants, DateTime startTime, SprintType type, string sprintLocation)
        {
            this.SprintInfo = new SprintInfoDto(sprintId, sprintName, distance, numberOfParticipants, startTime, type, sprintLocation);
            this.ParticipantInfo = new List<ParticipantInfoWithFriend>();
        }

        public SprintInfoDto SprintInfo { get; set; }
        public List<ParticipantInfoWithFriend> ParticipantInfo { get; set; }

        public void AddParticipant(int id, string name, string profilePicture, string city, string country,
            string countryCode, string colorCode, bool creator, ParticipantStage stage, bool isFriend)
        {
            this.ParticipantInfo.Add(new ParticipantInfoWithFriend(id, name, profilePicture, city, country, countryCode, colorCode, creator, stage, isFriend));
        }

    }

    public class ParticipantInfoWithFriend : ParticipantInfoDto
    {
        public ParticipantInfoWithFriend(int id, string name, string profilePicture, string city,
            string country, string countryCode, string colorCode, bool creator, ParticipantStage stage, bool isFriend) : base(id, name, profilePicture, city, country, countryCode, colorCode, creator, stage)
        {
            this.IsFriend = isFriend;
        }

        public bool IsFriend { get; }
    }
}