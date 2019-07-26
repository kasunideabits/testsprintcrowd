namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class SprintInvitationConfig : IEntityTypeConfiguration<SprintInvitation>
    {
        public void Configure(EntityTypeBuilder<SprintInvitation> builder)
        {
            builder
                .HasOne(s => s.Inviter)
                .WithOne(s => s.Inviter)
                .HasForeignKey<SprintInvitation>(s => s.InviterId);
            builder
                .HasOne(s => s.Inviter)
                .WithOne(s => s.Invitee)
                .HasForeignKey<SprintInvitation>(s => s.InviteeId);
            builder.Property<DateTime>("LastUpdated");
        }
    }
}