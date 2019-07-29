namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Entity configuration for SprintInvitationConfig table
    /// </summary>
    public class SprintInvitationConfig : IEntityTypeConfiguration<SprintInvitation>
    {
        /// <summary>
        /// Configure table SprintInvitation
        /// </summary>
        /// <param name="builder">entity builder instance</param>
        public void Configure(EntityTypeBuilder<SprintInvitation> builder)
        {
            builder
                .HasOne(s => s.Inviter)
                .WithOne(s => s.Inviter)
                .HasForeignKey<SprintInvitation>(s => s.InviterId);
            builder
                .HasOne(s => s.Invitee)
                .WithOne(s => s.Invitee)
                .HasForeignKey<SprintInvitation>(s => s.InviteeId);
            builder.Property<DateTime>("LastUpdated");
        }
    }
}