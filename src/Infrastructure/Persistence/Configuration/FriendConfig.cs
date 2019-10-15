namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
  using System;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Microsoft.EntityFrameworkCore;
  using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

  public class FriendConfig : IEntityTypeConfiguration<Friend>
  {
    public void Configure(EntityTypeBuilder<Friend> builder)
    {
      builder
          .HasOne(a => a.AcceptedUser)
          .WithMany(a => a.friendsAccepted)
          .HasForeignKey(a => a.AcceptedUserId);
      builder
          .HasOne(f => f.SharedUser)
          .WithMany(u => u.friendsShared)
          .HasForeignKey(f => f.SharedUserId);
      builder.Property<DateTime>("CreatedTime");
      builder.Property<DateTime>("UpdatedTime");
      builder.Property<DateTime>("LastUpdated");
    }
  }
}