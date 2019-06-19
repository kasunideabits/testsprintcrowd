namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    /// <summary>
    /// Entity configuration for AppDownloads table
    /// </summary>
    public class AppDowloadsConfig : IEntityTypeConfiguration<AppDownloads>
    {
        /// <summary>
        /// Configure table AppDownloads
        /// </summary>
        /// <param name="builder">entity builder instance</param>
        public void Configure(EntityTypeBuilder<AppDownloads> builder)
        {
            builder.Property<DateTime>("LastUpdated");
        }
    }
}