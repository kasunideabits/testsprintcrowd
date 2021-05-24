namespace SprintCrowd.BackEnd.Web.Event
{
    using System;
    using SprintCrowd.BackEnd.Domain.Sprint.Video;
    /// <summary>
    /// model for holding event data
    /// </summary>
    public class CreateSprintModel
    {
        /// <summary>
        /// number of participants for the sprint
        /// </summary>
        public int? NumberOfParticipants { get; set; }

        /// <summary>
        /// Event Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// smart link or invite
        /// </summary>
        public bool IsSmartInvite { get; set; }
        /// <summary>
        /// Influencer Availability
        /// </summary>
        public bool InfluencerAvailability { get; set; } = false;

        /// <summary>
        /// Influencer Email
        /// </summary>
        public string InfluencerEmail { get; set; }

        /// <summary>
        /// Event distance
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// Start Time of the event
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// event started or not
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// <see cref="SprintType">public or private</see>
        /// </summary>
        public int SprintType { get; set; }

        /// <summary>
        /// Draft or not
        /// </summary>
        public int DraftEvent { get; set; }
        /// <summary>
        /// ImageUrl
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// promotion Code
        /// </summary>
        public string promotionCode { get; set; }

        /// <summary>
        /// deternined if the sprint is time based or not
        /// </summary>
        public bool IsTimeBased { get; set; }

        // <summary>
        /// Duration for a time based event
        // </summary>
        public string DurationForTimeBasedEvent { get; set; }

        // <summary>
        /// Description for a time based event
        // </summary>
        public string DescriptionForTimeBasedEvent { get; set; }

        /// <summary>
        /// Social media sharble link
        /// </summary>
        public string SocialMediaLink { get; set; }

        public VideoType VideoType { get; set; }
        public string VideoLink { get; set; }

        public bool IsNarrationsOn { get; set; }
    }
}