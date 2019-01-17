using Microsoft.EntityFrameworkCore;

namespace SprintCrowdBackEnd.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfilePicture>()
            .HasKey(p => p.UserId);

            modelBuilder.Entity<ProfilePicture>()
            .HasOne(p => p.User)
            .WithOne(u => u.ProfilePicture)
            .HasForeignKey<ProfilePicture>(t => t.UserId);


        }
        public DbSet<User> Users { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }

    }
}