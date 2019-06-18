namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    public class AppDowloadsConfig : IEntityTypeConfiguration<AppDownloads>
    {
        public void Configure(EntityTypeBuilder<AppDownloads> builder)
        {
            builder.Property<DateTime>("LastUpdated");
        }
    }
}