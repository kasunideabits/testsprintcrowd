namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// Entity configuration for SprintParticipant table
    /// </summary>
    public class SprintParticipantConfig : IEntityTypeConfiguration<SprintParticipant>
    {
        /// <summary>
        /// Configure table SprintParticipant
        /// </summary>
        /// <param name="builder">entity builder instance</param>
        public void Configure(EntityTypeBuilder<SprintParticipant> builder)
        {
            builder
                .HasIndex(s => new { s.UserId, s.SprintId })
                .IsUnique();
            builder
                .HasOne(ep => ep.User)
                .WithMany(ep => ep.Participates)
                .HasForeignKey(ep => ep.UserId);
            builder
                .HasOne(ep => ep.Sprint)
                .WithMany(ep => ep.Participants);
        }
    }
}