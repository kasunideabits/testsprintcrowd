namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;

    public class UpdateSprintDto : SprintBaseDto
    {
        public UpdateSprintDto(
            int id,
            string name,
            int distance,
            int numberOfParticipants,
            DateTime startTime,
            SprintType type,
            int draftEvent,
            bool influencerAvailability,
            string influencerEmail) : base(id, name, distance, numberOfParticipants, startTime, type,string.Empty)
        {
            this.DraftEvent = draftEvent;
            this.InfluencerAvailability = influencerAvailability;
            this.influencerEmail = influencerEmail;
        }

        public int DraftEvent { get; }
        public bool InfluencerAvailability { get; }
        public string influencerEmail { get; }
    }
}