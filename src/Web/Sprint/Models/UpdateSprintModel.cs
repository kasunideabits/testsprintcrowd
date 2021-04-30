namespace SprintCrowd.BackEnd.Web.Event
{
    using System;
    using SprintCrowd.BackEnd.Domain.Sprint.Video;
    public class UpdateSprintModel
    {
        public string Name { get; set; }
        public int? NumberOfParticipants { get; set; }
        public int? Distance { get; set; }
        public DateTime? StartTime { get; set; }
        public int? Status { get; set; }
        public int? SprintType { get; set; }
        public string InfluencerEmail { get; set; }
        public int? DraftEvent { get; set; }
        public string ImageUrl { get; set; }
        public string promotionCode { get; set; }
        public bool InfluencerAvailability { get; set; }
        public bool IsTimeBased { get; set; }
        public string DurationForTimeBasedEvent { get; set; }
        public string DescriptionForTimeBasedEvent { get; set; }
        public VideoType VideoType { get; set; }
        public string VideoLink { get; set; }
    }
}