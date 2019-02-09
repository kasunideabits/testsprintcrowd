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
        /// table for events.
        /// </summary>
        public DbSet<Event> Event { get; set; }
        /// <summary>
        /// table for tracking participants.
        /// </summary>
        public DbSet<EventParticipant> EventParticipant { get; set; }

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
                .HasMany(u => u.Events);
            builder.Entity<Event>()
                .HasOne(e => e.CreatedBy);
            builder.Entity<Event>()
                .HasMany(e => e.Participants);
            builder.Entity<EventParticipant>()
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
            builder.Entity<EventParticipant>()
                .Property<DateTime>("LastUpdated");
            builder.Entity<Event>()
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