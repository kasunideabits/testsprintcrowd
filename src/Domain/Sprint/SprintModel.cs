namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System;
    /// <summary>
    /// model for holding event data
    /// </summary>
    public class SprintModel
    {
        /// <inheritdoc />
        public SprintModel(
            string name,
            int distance,
            string location,
            DateTime startTime,
            int sprintType,
            double lattitude,
            double longitutude,
            int id,
            int numOfParticiapants,
            bool influencerAvailability,
            string influencerEmail,
            int draftEvent)
        {
            this.Name = name;
            this.Distance = distance;
            this.Location = location;
            this.StartTime = startTime;
            this.SprintType = sprintType;
            this.Lattitude = lattitude;
            this.Longitutude = longitutude;
            this.Id = id;
            this.NumberOfParticipants = numOfParticiapants;
            this.InfluencerAvailability = influencerAvailability;
            this.InfluencerEmail = influencerEmail;
            this.DraftEvent = draftEvent;
        }

        /// <summary>
        /// Event Id
        /// </summary>
        /// <value>Sprint Id</value>
        public int Id { get; }

        /// <summary>
        /// number of participants for the sprint
        /// </summary>
        /// <value>number of participants for sprint</value>
        public int NumberOfParticipants { get; set; }

        /// <summary>
        /// Event Name
        /// </summary>
        /// <value>Name of the sprint</value>
        public string Name { get; set; }

        /// <summary>
        /// Influencer Availability
        /// </summary>
        /// <value>Influencer Availability</value>
        public bool InfluencerAvailability { get; set; }

        /// <summary>
        /// Influencer Email
        /// </summary>
        /// <value>Influencer Email</value>
        public string InfluencerEmail { get; set; }

        /// <summary>
        /// Event distance
        /// </summary>
        /// <value>Distance in meters for the sprint</value>
        public int Distance { get; set; }

        /// <summary>
        /// Gets or set location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Start Time of the event
        /// </summary>
        /// <value>sprint start time</value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// event started or not
        /// </summary>
        /// <value>events started or not</value>
        public int Status { get; set; }

        /// <summary>
        /// public or private
        /// </summary>
        /// <value>sprint type, public or private</value>
        public int SprintType { get; set; }

        /// <summary>
        /// Latitutude
        /// </summary>
        /// <value>lattitude</value>
        public double Lattitude { get; set; }

        /// <summary>
        /// Longitutude
        /// </summary>
        /// <value>longitutude</value>
        public double Longitutude { get; set; }

        /// <summary>
        /// Draft or not
        /// </summary>
        /// <value>Draft Event status</value>
        public int DraftEvent { get; set; }

    }
}