
namespace SprintCrowdBackEnd.Persistence
{
    using System;
    using SprintCrowdBackEnd.Models;

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLoggedInTime { get; set; }
        public string Email { get; set; }
        public string FbUserId { get; set; }
        public string Token { get; set; }
        public ProfilePicture ProfilePicture { get; set; }
    }
}