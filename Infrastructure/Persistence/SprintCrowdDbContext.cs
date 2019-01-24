namespace SprintCrowd.Backend.Infrastructure.Persistence
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using SprintCrowd.Backend.Domain;

    public class SprintCrowdDbContext : IdentityUserContext<ApplicationUser, Guid>
    {
        public SprintCrowdDbContext(DbContextOptions<SprintCrowdDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}