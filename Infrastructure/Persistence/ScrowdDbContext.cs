namespace SprintCrowdBackEnd.Infrastructure.Persistence
{
    using System.ComponentModel.DataAnnotations.Schema;
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
                .HasMany(u => u.Events);
            builder.Entity<Event>()
                .HasOne(e => e.CreatedBy);
            builder.Entity<Event>()
                .HasMany(e => e.Participants);
            builder.Entity<EventParticipant>()
                .HasOne(ep => ep.User);
        }
    }
}