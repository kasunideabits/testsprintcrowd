namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
  using System;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Microsoft.EntityFrameworkCore;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  /// Entity configuration for Achievement table
  /// </summary>
  public class AchievementConfig : IEntityTypeConfiguration<Achievement>
  {
    /// <summary>
    /// Configure table Achievement
    /// </summary>
    /// <param name="builder">entity builder instance</param>
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
      builder.HasOne(a => a.User);
      builder.Property<DateTime>("LastUpdated");
    }
  }
}