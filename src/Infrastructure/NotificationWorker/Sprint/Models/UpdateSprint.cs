using System;
using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker.Sprint.Models
{
    public class UpdateSprint
    {
        public UpdateSprint(int sprintId, string sprintName, int distance, DateTime startTime, int numberOfParticipant, SprintStatus sprintStatus, SprintType sprintType, int creatorId)
        {
            this.SprintId = sprintId;
            this.SprintName = sprintName;
            this.Distance = distance;
            this.StartTime = startTime;
            this.NumberOfParticipant = numberOfParticipant;
            this.SprintStatus = sprintStatus;
            this.SprintType = sprintType;
            this.CreatorId = creatorId;
        }

        public int SprintId { get; }
        public string SprintName { get; }
        public int Distance { get; }
        public DateTime StartTime { get; }
        public int NumberOfParticipant { get; }
        public SprintStatus SprintStatus { get; }
        public SprintType SprintType { get; }
        public int CreatorId { get; }

    }
}