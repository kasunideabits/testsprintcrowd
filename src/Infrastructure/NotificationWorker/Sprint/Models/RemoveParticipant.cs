namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models
{
    using System;
    using SprintCrowd.BackEnd.Application;

    public class RemoveParticipant
    {
        public RemoveParticipant(int sprintId, SprintType sprintType, SprintStatus sprintStatus, int creatorId, int userId,
                                string creatorName, string sprintName, DateTime startTime, int numOfparticipant, int distance,
                                string name, string profilePicture, string code, string country, string countryCode, string city)
        {
            this.SprintId = sprintId;
            this.SprintName = sprintName;
            this.SprintType = sprintType;
            this.SprintStatus = sprintStatus;
            this.CreatorId = creatorId;
            this.CreatorName = creatorName;
            this.UserId = userId;
            this.StartTime = startTime;
            this.NumberOfParticipant = numOfparticipant;
            this.Distance = distance;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.Code = code;
            this.Country = country;
            this.City = city;
            this.CountryCode = countryCode;

        }


        public int SprintId { get; }
        public SprintType SprintType { get; }
        public SprintStatus SprintStatus { get; }
        public int CreatorId { get; }
        public string CreatorName { get; }
        public int UserId { get; }
        public string SprintName { get; }
        public DateTime StartTime { get; }
        public int NumberOfParticipant { get; }
        public int Distance { get; }
        public string Name { get; }
        public string ProfilePicture { get; }
        public string Code { get; }
        public string Country { get; }
        public string CountryCode { get; }
        public string City { get; }

    }


}