using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Common;

namespace SprintCrowdBackEnd.Domain.Sprint.Dtos
{
    public class SprintWithParticipantProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime StartTime { get; set; }
        public SprintType Type { get; set; }
        public string ImageUrl { get; set; }
        public string PromoCode { get; set; }
        public bool IsTimeBased { get; set; }
        public TimeSpan durationForTimeBasedEvent { get; set; }
        public List<ParticipantProfile> Participants { get; set; }

    }

    public class ParticipantProfile
    {
        public string UserName { get; set; }
        public string ProfilePicture { get; set; }
        public int UserId { get; set; }
        public int Position { get; set; }
        public string RaceCompletedDuration { get; set; }
        public double Distance { get; set; }

    }
}
