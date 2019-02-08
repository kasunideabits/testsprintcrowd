namespace SprintCrowdBackEnd.Infrastructure.Persistence.Entities
{
    public class User
    {
        public int Id {get; set;}
        public int FacebookUserId {get; set;}
        public string Email {get; set;}
        public string Name {get; set;}
        public string ProfilePicture {get; set;}
    }
}