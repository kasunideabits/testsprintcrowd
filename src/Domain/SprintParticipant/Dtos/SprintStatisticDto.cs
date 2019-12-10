using System;

namespace SprintCrowd.BackEnd.Domain.SprintParticipant.Dtos
{
    public class SprintStatisticDto
    {
        public SprintStatisticDto()
        {
            this.PublicEvent = new EventStatDto();
            this.PrivateEvent = new EventStatDto();
        }

        public EventStatDto PublicEvent { get; set; }
        public EventStatDto PrivateEvent { get; set; }

        public void SetPublicEvent(Infrastructure.Persistence.Entities.SprintParticipant participate)
        {
            this.PublicEvent.TotalDistance += participate.DistanceRan;
            this.PublicEvent.TotalTime += CalcTime(participate.StartedTime, participate.FinishTime);
        }

        public void SetPrivateEvent(Infrastructure.Persistence.Entities.SprintParticipant participate)
        {
            this.PrivateEvent.TotalDistance += participate.DistanceRan;
            this.PrivateEvent.TotalTime += CalcTime(participate.StartedTime, participate.FinishTime);
        }

        private static int CalcTime(DateTime startTime, DateTime finishTime)
        {
            TimeSpan timeSpend = finishTime - startTime;
            return (int)timeSpend.TotalMinutes;
        }
    }

    public class EventStatDto
    {
        public int TotalDistance { get; set; }
        public int TotalTime { get; set; }
    }
}