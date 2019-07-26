namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Entity configuration for SprintInvitiation table
    /// </summary>
    public class SprintInvitationNotificationConfig : IEntityTypeConfiguration<SprintInvitationNotification>
    {
        /// <summary>
        /// Configure table SprintInvitationNotification
        /// </summary>
        /// <param name="builder">entity builder instance</param>
        public void Configure(EntityTypeBuilder<SprintInvitationNotification> builder)
        {
            builder
                .HasOne(s => s.Sprint)
                .WithMany(s => s.SprintInvitationNotification)
                .HasForeignKey(s => s.SprintId);
            builder
                .HasOne(s => s.Notification)
                .WithOne(s => s.SprintInvitation)
                .HasForeignKey<SprintInvitationNotification>(s => s.NotificationId);
            builder.Property<DateTime>("LastUpdated");
        }
    }
}