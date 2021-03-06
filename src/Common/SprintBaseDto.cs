namespace SprintCrowd.BackEnd.Common
{
    using System;
    using SprintCrowd.BackEnd.Application;

    public class SprintBaseDto
    {
        public SprintBaseDto(int id, string name, int distance, int numberOfParticipants, DateTime startTime, SprintType type,string imageUrl , string promoCode)
        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.NumberOfParticipants = numberOfParticipants;
            this.StartTime = startTime;
            this.Type = type;
            this.ImageUrl = imageUrl;
            this.PromoCode = promoCode;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime StartTime { get; set; }
        public SprintType Type { get; set; }
        public string ImageUrl { get; set; }
        public string PromoCode { get; set; }
    }
}