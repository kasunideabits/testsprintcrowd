namespace SprintCrowd.BackEnd.Web.SprintManager
{
    public class NotCompletedRunners
    {
        public int UserId { get; }

        public string ProfilePicture { get; }

        public string Name { get; }

        public string Country { get; }

        public string CountryCode { get; }

        public string ColorCode { get; }

        public string City { get; }

        public double DistanceRun { get; }

        public double Position { get; }

        public bool RaceCompleted { get; }

        public string RaceCompletedDuration { get; }

        public bool Exited { get; set; }
    }
}