using System;

namespace SprintCrowdBackEnd.Infrastructure.Persistence.Entities
{
    public class Event
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public int Distance {get; set;}
        public User CreatedBy {get; set;}
        public DateTime StartDateTime {get; set;}
        // event type public or not
        public int Type {get; set;}
        // event status
        public int Status {get; set;}
        public bool LocationProvided {get; set;}
        public double Lattitude {get; set;}
        public double Longitutude {get; set;}
    }
}