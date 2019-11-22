namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;

    public class SprintInfoDto : SprintBaseDto
    {
        public SprintInfoDto(
            int id,
            string name,
            int distance,
            int numberOfParticipants,
            DateTime startTime,
            SprintType type,
            string location = null) : base(id, name, distance, numberOfParticipants, startTime, type)
        {
            this.Location = location;
            this.ExtendedTime = startTime.AddMinutes(15);
        }

        public string Location { get; }
        public DateTime ExtendedTime { get; }
    }
}