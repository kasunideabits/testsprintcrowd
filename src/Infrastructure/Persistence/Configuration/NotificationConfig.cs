namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Entity configuration for Notifications table
    /// </summary>
    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        /// <summary>
        /// Configure table Notifications
        /// </summary>
        /// <param name="builder">entity builder instance</param>
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
                .HasDiscriminator<NotificationType>("Type")
                .HasValue<SprintNotification>(NotificationType.SprintNotification)
                .HasValue<FriendNoticiation>(NotificationType.FriendNotification)
                .HasValue<AchievementNoticiation>(NotificationType.AchivementNotification);
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