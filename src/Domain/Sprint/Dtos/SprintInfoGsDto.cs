namespace SprintCrowd.BackEnd.Domain.Sprint.Dtos
{
    using System;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Common;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class SprintInfoGsDto 
    {
        public SprintInfoGsDto(
            int id,
            string name,
            int distance,
            string promoCode,
            DateTime startTime,
            int type,
            int interval ,
            string descriptionForTimeBasedEvent = null            
            ) 

        {
            this.Id = id;
            this.Name = name;
            this.Distance = distance;
            this.PromoCode = promoCode;
            this.StartTime = startTime;
            this.Type = type;
            this.ExtendedTime = startTime.AddMinutes(interval);
            this.DescriptionForTimeBasedEvent = descriptionForTimeBasedEvent;
            

        }

        public int Id { get; set; }
        public string Name { get; set; } 
        public int Distance { get; set; }
        public string PromoCode { get; set; }
        public DateTime StartTime { get; set; }
        public int Type { get; set; }
        public DateTime ExtendedTime { get; }
        public string DescriptionForTimeBasedEvent { get; set; }
       

    }
}