namespace SprintCrowd.BackEnd.Domain.SprintParticipant
{
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class NotificationInfo //<T> where T : class, new()
    {
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public Notification Notification { get; set; }

    }
}