namespace SprintCrowdBackEnd.Infrastructure.Persistence
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using SprintCrowd.Backend;

    /// <summary>
    /// Design-time DB context factory.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ScrowdDbContext>
    {
        /// <inheritdoc />
        public ScrowdDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScrowdDbContext>();
            optionsBuilder.UseNpgsql(Program.Configuration.GetConnectionString("SprintCrowd"));
            return new ScrowdDbContext(optionsBuilder.Options);
        }
    }
}