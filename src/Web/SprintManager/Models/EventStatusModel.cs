namespace SprintCrowd.BackEnd.SprintManager.Web
{
    using System;

    public class EventStatusModel
    {
        public int UserId { get; set; }
        public int SprintId { get; set; }
        public int Distance { get; set; }
        public DateTime Time { get; set; }
        public double Position { get; set; }
    }
}