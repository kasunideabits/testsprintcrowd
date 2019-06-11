namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
  using System;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Microsoft.EntityFrameworkCore;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  /// Entity configuration for Sprint table
  /// </summary>
  public class SprintConfig : IEntityTypeConfiguration<Sprint>
  {
    /// <summary>
    /// Configure table Sprint
    /// </summary>
    /// <param name="builder">entity builder instance</param>
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
      builder.HasOne(s => s.CreatedBy).WithMany(s => s.Sprint);
      builder.HasMany(s => s.Participants);
      builder.Property<DateTime>("LastUpdated");
    }
  }
}