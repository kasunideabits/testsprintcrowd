namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class SprintInviteConfig : IEntityTypeConfiguration<SprintInvite>
    {
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
            builder.Property<DateTime>("LastUpdated");
        }
    }
}