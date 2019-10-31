namespace SprintCrowd.BackEnd.Infrastructure.Persistence
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Factory which responsible for create database context
    /// </summary>
    public class ScrowdDbFactory : IDisposable
    {
        /// <summary>
        /// Create new <see cref="ScrowdDbContext"> db context </see>
        /// </summary>
        /// <returns>new db context</returns>
        public ScrowdDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScrowdDbContext>();
            optionsBuilder.UseNpgsql(GetConnectionString());
            this.Context = new ScrowdDbContext(optionsBuilder.Options);
            return this.Context;
        }

        private ScrowdDbContext Context { get; set; }

        /// <summary>
        /// Get databa connection string
        /// </summary>
        /// <returns>Connection string</returns>
        private static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            return configuration.GetValue<string>("ConnectionStrings:SprintCrowd");
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}