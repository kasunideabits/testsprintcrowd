namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
  using System;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Microsoft.EntityFrameworkCore;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  /// <summary>
  /// Entity configuration for UserActivity table
  /// </summary>
  public class UserActivityConfig : IEntityTypeConfiguration<UserActivity>
  {
    /// <summary>
    /// Configure table UserActivity
    /// </summary>
    /// <param name="builder">entity builder instance</param>
    public void Configure(EntityTypeBuilder<UserActivity> builder)
    {
      builder
        .HasOne(a => a.User)
        .WithMany(a => a.UserActivity)
        .HasForeignKey(a => a.UserId);
    }
  }
}