using System.Collections.Generic;

namespace SprintCrowdBackEnd.Infrastructure.Persistence.Entities
{
    public class User
    {
        public int Id {get; set;}
        public string FacebookUserId {get; set;}
        public string Email {get; set;}
        public string Name {get; set;}
        public string ProfilePicture {get; set;}
        public List<Event> Events {get; set;}
    }
}