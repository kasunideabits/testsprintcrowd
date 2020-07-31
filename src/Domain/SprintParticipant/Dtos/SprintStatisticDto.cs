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
            if (participate.DistanceRan > 0)
            {
                this.PublicEvent.TotalCompleted += 1;
                this.PublicEvent.TotalDistance += participate.DistanceRan;
                this.PublicEvent.TotalTime += CalcTime(participate.StartedTime, participate.FinishTime);
            }
        }

        public void SetPrivateEvent(Infrastructure.Persistence.Entities.SprintParticipant participate)
        {
            if (participate.DistanceRan > 0)
            {
                this.PrivateEvent.TotalCompleted += 1;
                this.PrivateEvent.TotalDistance += participate.DistanceRan;
                this.PrivateEvent.TotalTime += CalcTime(participate.StartedTime, participate.FinishTime);
            }
        }

        private static int CalcTime(DateTime startTime, DateTime finishTime)
        {
            TimeSpan timeSpend = finishTime - startTime;
            return (int)timeSpend.TotalMinutes;
        }
    }

    public class EventStatDto
    {
        public int TotalCompleted { get; set; }
        public int TotalDistance { get; set; }
        public int TotalTime { get; set; }
    }
}