namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    public class UserNotificationReminder : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool TwentyFourH { get; set; } = true;
        public bool OneH { get; set; } = false;
        public bool FiftyM { get; set; } = true;
        public bool EventStart { get; set; } = true;
        public bool FinalCall { get; set; } = true;
        public virtual User User { get; set; }
    }
}