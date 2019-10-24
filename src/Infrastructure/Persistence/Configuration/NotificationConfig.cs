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
                .HasOne(s => s.Sender)
                .WithMany(s => s.SenderNotification)
                .HasForeignKey(s => s.SenderId);
            builder
                .HasOne(s => s.Receiver)
                .WithMany(s => s.ReceiverNotification)
                .HasForeignKey(s => s.ReceiverId);
            builder
                .HasOne(s => s.Achievement)
                .WithMany(s => s.Notificatoins)
                .HasForeignKey(s => s.AchievementId);
            builder
                .HasOne(n => n.SprintInvite)
                .WithMany(s => s.Notification)
                .HasForeignKey(n => n.SprintInviteId);
        }
    }
}