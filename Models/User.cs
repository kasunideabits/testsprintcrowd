using System;

namespace SprintCrowdBackEnd.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLoggedInTime { get; set; }
        public string Email { get; set; }
        public string FbUserId { get; set; }
        //No password needed as logging from facebook
        //public string Password { get; set; }
        public string Token { get; set; }
    }
}