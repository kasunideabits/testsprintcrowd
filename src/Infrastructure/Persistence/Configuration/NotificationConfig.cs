using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    /// <summary>
    /// Entity configuration for Notifications table
    /// </summary>
    public class NotificationConfig : IEntityTypeConfiguration<Notifications>
    {
        /// <summary>
        /// Configure table Notifications
        /// </summary>
        /// <param name="builder">entity builder instance</param>
        public void Configure(EntityTypeBuilder<Notifications> builder)
        {
            builder
                .HasOne(s => s.Sender)
                .WithMany(s => s.SenderNotification)
                .HasForeignKey(s => s.SenderId);
            builder
                .HasOne(s => s.Receiver)
                .WithMany(s => s.ReceiverNotification)
                .HasForeignKey(s => s.ReceiverId);
            builder.Property<DateTime>("LastUpdated");
        }
    }
}