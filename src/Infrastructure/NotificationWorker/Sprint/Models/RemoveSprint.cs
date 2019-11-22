using System;
using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models
{
    public class RemoveSprint
    {
        public RemoveSprint(int sprintId, string sprintName, int distance, DateTime startTime, int numberOfParticipant, SprintStatus sprintStatus, SprintType sprintType, int userId, string name, string profilePicture, string code, string colorCode, string city, string country, string countryCode)
        {
            this.SprintId = sprintId;
            this.SprintName = sprintName;
            this.Distance = distance;
            this.StartTime = startTime;
            this.NumberOfParticipant = numberOfParticipant;
            this.SprintStatus = sprintStatus;
            this.SprintType = sprintType;
            this.UserId = userId;
            this.Name = name;
            this.ProfilePicture = profilePicture;
            this.ColorCode = colorCode;
            this.Code = code;
            this.City = city;
            this.Country = country;
            this.CountryCode = countryCode;

        }
        public int SprintId { get; }
        public string SprintName { get; }
        public int Distance { get; }
        public DateTime StartTime { get; }
        public int NumberOfParticipant { get; }
        public SprintStatus SprintStatus { get; }
        public SprintType SprintType { get; }
        public int UserId { get; }
        public string Name { get; }
        public string ProfilePicture { get; }
        public string Code { get; }
        public string ColorCode { get; }
        public string City { get; }
        public string Country { get; }
        public string CountryCode { get; }
    }
}