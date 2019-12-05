namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    public class UserNotificationReminder : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool TwentyForH { get; set; } = false;
        public bool OneH { get; set; } = false;
        public bool FiftyM { get; set; } = false;
        public bool EventStart { get; set; } = false;
        public bool FinalCall { get; set; } = false;
        public virtual User User { get; set; }
    }
}