namespace SprintCrowdBackEnd.Infrastructure.Persistence.Entities
{
    public class EventParticipant
    {
        public int Id {get; set;}
        public User User {get; set;}
        //participant joined or marked attendence
        public int Stage {get; set;}
    }
}