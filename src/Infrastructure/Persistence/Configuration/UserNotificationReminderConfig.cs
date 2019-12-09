namespace SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    public class UserNotificationReminderConfig : IEntityTypeConfiguration<UserNotificationReminder>
    {
        public void Configure(EntityTypeBuilder<UserNotificationReminder> builder)
        {
            builder
                .HasOne(u => u.User)
                .WithOne(u => u.UserNotificationReminder)
                .HasForeignKey<UserNotificationReminder>(u => u.UserId);
        }
    }
}