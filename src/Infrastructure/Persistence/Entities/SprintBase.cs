using System;
using SprintCrowd.BackEnd.Application;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    public class SprintBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public DateTime StartDateTime { get; set; }
        public SprintType Type { get; set; }
        public SprintStatus Status { get; set; }
        public int NumberOfParticipants { get; set; }
    }
}