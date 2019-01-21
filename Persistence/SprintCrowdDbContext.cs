
namespace SprintCrowdBackEnd.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using SprintCrowdBackEnd.Persistence.Configuration;

    public class SprintCrowdDbContext : DbContext
    {
        public SprintCrowdDbContext(DbContextOptions<SprintCrowdDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                builder.ApplyConfiguration(new UserConfig());
                builder.ApplyConfiguration(new ProfilePictureConfig());
            }
    }

   
}