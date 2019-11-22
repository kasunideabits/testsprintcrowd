namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class UserNotificationConfig : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder
                .HasOne(u => u.Notification)
                .WithMany(u => u.UserNotification)
                .HasForeignKey(u => u.NotificationId);
            builder
                .HasOne(n => n.Sender)
                .WithMany(u => u.SenderNotification)
                .HasForeignKey(n => n.SenderId);
            builder
                .HasOne(n => n.Receiver)
                .WithMany(u => u.ReceiverNotification)
                .HasForeignKey(n => n.ReceiverId);
        }
    }
}