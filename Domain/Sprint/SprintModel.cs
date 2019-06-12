namespace SprintCrowd.BackEnd.Domain.Sprint
{
    using System;
    /// <summary>
    /// model for holding event data
    /// </summary>
    public class SprintModel
    {

        /// <inheritdoc />
        public SprintModel(string name, int distance, Boolean locationProvided, DateTime startTime, int sprintType,
            double lattitude, double longitutude, int id, int numOfParticiapants)
        {
            this.Name = name;
            this.Distance = distance;
            this.LocationProvided = locationProvided;
            this.StartTime = startTime;
            this.SprintType = sprintType;
            this.Lattitude = lattitude;
            this.Longitutude = longitutude;
            this.Id = id;
            this.NumberOfParticipants = numOfParticiapants;
        }

        /// <summary>
        /// Event Id
        /// </summary>
        /// <value></value>
        public int Id { get; }
        /// <summary>
        /// number of participants for the sprint
        /// </summary>
        /// <value></value>
        public int NumberOfParticipants { get; set; }
        /// <summary>
        /// Event Name
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// Event distance
        /// </summary>
        /// <value></value>
        public int Distance { get; set; }
        /// <summary>
        /// Wether location is provided or not
        /// </summary>
        /// <value></value>
        public Boolean LocationProvided { get; set; }
        /// <summary>
        /// Start Time of the event
        /// </summary>
        /// <value></value>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// event started or not
        /// </summary>
        /// <value></value>
        public int Status { get; set; }
        /// <summary>
        /// public or private
        /// </summary>
        /// <value></value>
        public int SprintType { get; set; }
        /// <summary>
        /// Latitutude
        /// </summary>
        /// <value></value>
        public double Lattitude { get; set; }
        /// <summary>
        /// Longitutude
        /// </summary>
        /// <value></value>
        public double Longitutude { get; set; }
    }
}