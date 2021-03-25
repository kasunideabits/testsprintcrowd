using System;
using System.Collections.Generic;
using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    public class JoinedSprintDto
    {
        public JoinedSprintDto(Infrastructure.Persistence.Entities.Sprint sprint, int totalNumberOfParticipants)
        {
            this.Id = sprint.Id;
            this.Name = sprint.Name;
            this.Status = (SprintStatus)sprint.Status;
            this.Type = (SprintType)sprint.Type;
            this.NumberOfParticipants = sprint.NumberOfParticipants;
            this.TotalNumberOfParticiapnts = totalNumberOfParticipants;
            this.Distance = sprint.Distance;
            this.StartTime = sprint.StartDateTime;
            this.CreatedBy = (sprint.CreatedBy != null) ? sprint.CreatedBy.Id : 0;
            this.PromoCode = sprint.PromotionCode;
        }
        public int Id { get; }
        public string Name { get; }
        public SprintStatus Status { get; }
        public SprintType Type { get; }
        public int NumberOfParticipants { get; }
        public DateTime StartTime { get; }
        public int TotalNumberOfParticiapnts { get; }
        public int Distance { get; }
        public int CreatedBy { get; }
        public string PromoCode { get; }

    }
}