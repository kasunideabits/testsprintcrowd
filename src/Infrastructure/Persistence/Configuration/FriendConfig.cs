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
            builder.HasOne(f => f.User).WithMany(u => u.Friends).HasForeignKey(f => f.FriendId);
            builder
                .HasOne(f => f.FriendOf)
                .WithMany(u => u.FriendRequester)
                .HasForeignKey(f => f.UserId);
            builder.Property<DateTime>("LastUpdated");
        }
    }
}