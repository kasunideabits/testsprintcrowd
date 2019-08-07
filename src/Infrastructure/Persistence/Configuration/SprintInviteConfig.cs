namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Entity configuration for SprintInvite table
    /// </summary>
    public class SprintInviteConfig : IEntityTypeConfiguration<SprintInvite>
    {
        /// <summary>
        /// Configure table SprintInvite
        /// </summary>
        /// <param name="builder">entity builder instance</param>
        public void Configure(EntityTypeBuilder<SprintInvite> builder)
        {
            builder
                .HasOne(s => s.Invitee)
                .WithOne(s => s.Invitee)
                .HasForeignKey<SprintInvite>(s => s.InviteeId);
            builder
                .HasOne(s => s.Inviter)
                .WithOne(s => s.Inviter)
                .HasForeignKey<SprintInvite>(s => s.InviterId);
            builder
                .HasOne(s => s.Sprint)
                .WithMany(s => s.SprintInvites)
                .HasForeignKey(s => s.SprintId);
            builder.Property<DateTime>("LastUpdated");
        }
    }
}