namespace SprintCrowd.BackEnd.Infrastructure.Persistence
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Npgsql.NameTranslation;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Configuration;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// db context for sprintcrowdbackend.
    /// </summary>
    public class ScrowdDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrowdDbContext"/> class.
        /// </summary>
        public ScrowdDbContext(DbContextOptions<ScrowdDbContext> options) : base(options) { }

        /// <summary>
        /// table for users.
        /// </summary>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// table for sprints.
        /// </summary>
        public DbSet<Sprint> Sprint { get; set; }
        /// <summary>
        /// table for tracking participants.
        /// </summary>
        public DbSet<SprintParticipant> SprintParticipant { get; set; }

        /// <summary>
        /// table for firebase cloud messaging tokens
        /// </summary>
        public DbSet<FirebaseMessagingToken> FirebaseToken { get; set; }
        /// <summary>
        /// table for achivements
        /// </summary>
        public DbSet<Achievement> Achievement { get; set; }
        /// <summary>
        /// table for app downloads
        /// </summary>
        public DbSet<AppDownloads> AppDownloads { get; set; }

        /// <summary>
        /// table for notifications
        /// </summary>
        public DbSet<Notification> Notification { get; set; }

        /// <summary>
        /// table for track friends
        /// </summary>
        public DbSet<Friend> Frineds { get; set; }

        /// <summary>
        ///  table for sprint invitation
        /// </summary>
        /// <value></value>
        public DbSet<SprintInvite> SprintInvite { get; set; }

        public DbSet<SprintNotification> SprintNotifications { get; set; }

        public DbSet<UserNotification> UserNotification { get; set; }

        public DbSet<FriendNoticiation> FriendNoticiations { get; set; }

        public DbSet<AchievementNoticiation> AchievementNoticiations { get; set; }

        /// <summary>
        /// override save changes to insert last updated value.
        /// </summary>
        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Property("LastUpdated").CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        /// <summary>
        /// define table relationships.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AchievementConfig());
            builder.ApplyConfiguration(new FirebaseMessagingTokenConfig());
            builder.ApplyConfiguration(new SprintConfig());
            builder.ApplyConfiguration(new SprintParticipantConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new AppDowloadsConfig());
            builder.ApplyConfiguration(new NotificationConfig());
            builder.ApplyConfiguration(new FriendConfig());
            builder.ApplyConfiguration(new SprintInviteConfig());
            builder.ApplyConfiguration(new UserNotificationConfig());
        }

        private void FixSnakeCaseNames(ModelBuilder modelBuilder)
        {
            var mapper = new NpgsqlSnakeCaseNameTranslator();

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // modify column names
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = mapper.TranslateMemberName(property.Relational().ColumnName);
                }

                // modify table name
                entity.Relational().TableName = mapper.TranslateMemberName(entity.Relational().TableName);
            }
        }
    }
}