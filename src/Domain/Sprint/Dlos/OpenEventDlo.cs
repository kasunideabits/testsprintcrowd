namespace SprintCrowd.BackEnd.Domain.Sprint.Dlos
{
    using System.Collections.Generic;

    public class OpenEventDlo
    {
        public OpenEventDlo(
            Infrastructure.Persistence.Entities.Sprint sprint,
            IEnumerable<Infrastructure.Persistence.Entities.User> participants)
        {
            this.Sprint = sprint;
            this.Participants = participants;
        }
        public Infrastructure.Persistence.Entities.Sprint Sprint { get; set; }
        public IEnumerable<Infrastructure.Persistence.Entities.User> Participants { get; set; }
    }
}