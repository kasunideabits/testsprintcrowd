using Microsoft.EntityFrameworkCore;
using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;

namespace SprintCrowdBackEnd.Infrastructure.Persistence
{
    public class ScrowdDbContext : DbContext
    {
        public DbSet<User> User {get; set;}
        public DbSet<Event> Event {get; set;}
        public DbSet<EventParticipant> EventParticipant {get; set;}

        public ScrowdDbContext(DbContextOptions<ScrowdDbContext> options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.Events);
            builder.Entity<Event>()
                .HasOne(e => e.CreatedBy);
            builder.Entity<EventParticipant>()
                .HasOne(ep => ep.User);
        }
    }
}