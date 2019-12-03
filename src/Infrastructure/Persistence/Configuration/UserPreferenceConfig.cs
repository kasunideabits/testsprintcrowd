namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class UserPreferenceConfig : IEntityTypeConfiguration<UserPreference>
    {
        public void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            builder
                .HasOne(u => u.User)
                .WithOne(u => u.UserPreference)
                .HasForeignKey<UserPreference>(u => u.UserId);
        }
    }
}