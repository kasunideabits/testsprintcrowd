namespace SprintCrowdBackEnd.Infrastructure.Persistence
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

    /// <summary>
    /// db context for sprintcrowdbackend.
    /// </summary>
    public class ScrowdDbContext : DbContext
    {
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
        /// Initializes a new instance of the <see cref="ScrowdDbContext"/> class.
        /// </summary>
        public ScrowdDbContext(DbContextOptions<ScrowdDbContext> options) : base(options) { }

        /// <summary>
        /// define table relationships.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            builder.Entity<User>()
                .HasOne(u => u.AccessToken);
            builder.Entity<User>()
                .HasMany(u => u.Sprint);
            builder.Entity<Sprint>()
                .HasOne(e => e.CreatedBy);
            builder.Entity<Sprint>()
                .HasMany(e => e.Participants);
            builder.Entity<SprintParticipant>()
                .HasOne(ep => ep.User);
            this.SetShadowProperties(builder);
        }

        /// <summary>
        /// sets shadow properties.
        /// </summary>
        /// <param name="builder">model builder.</param>
        protected void SetShadowProperties(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property<DateTime>("LastUpdated");
            builder.Entity<AccessToken>()
                .Property<DateTime>("LastUpdated");
            builder.Entity<SprintParticipant>()
                .Property<DateTime>("LastUpdated");
            builder.Entity<Sprint>()
                .Property<DateTime>("LastUpdated");
        }

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
    }
}