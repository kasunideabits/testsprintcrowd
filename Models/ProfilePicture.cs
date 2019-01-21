namespace SprintCrowdBackEnd.Models
{
    using SprintCrowdBackEnd.Persistence;

    public class ProfilePicture
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}