namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class UserAchievementConfig : IEntityTypeConfiguration<UserAchievement>
    {
        public void Configure(EntityTypeBuilder<UserAchievement> builder)
        {
            builder
                .HasOne(u => u.User)
                .WithMany(u => u.UserAchievements)
                .HasForeignKey(u => u.UserId);
        }
    }
}