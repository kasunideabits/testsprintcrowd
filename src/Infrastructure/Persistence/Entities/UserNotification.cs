using System.ComponentModel;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Entities
{
    public class UserNotification : BaseEntity
    {
        public int Id { get; set; }
        public int? SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int NotificationId { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public virtual Notification Notification { get; set; }
        
        [DefaultValue(1)]
        public int BadgeValue { get; set; }

        [DefaultValue(false)]
        public bool IsCommunity { get; set; }
    }
}