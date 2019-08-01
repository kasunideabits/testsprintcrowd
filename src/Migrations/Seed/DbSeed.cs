namespace SprintCrowd.BackEnd.Migrations.Seed
{
    using System.Linq;
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;

    /// <summary>
    /// auth DB seeder.
    /// </summary>
    public class DbSeed
    {
        /// <summary>
        /// create default auth user for control-panel
        /// </summary>
        public static void InitializeData(IServiceProvider services)
        {
            var context = services.GetRequiredService<ScrowdDbContext>();

            const string adminUserEmail = "admin@sprintcrowd.se";
            var admin = context.User.Where(s => s.Email == adminUserEmail).FirstOrDefault();

            if (admin == null)
            {
                User user = new User()
                {
                UserType = (int)UserType.AdminUser,
                Email = adminUserEmail,
                FacebookUserId = "SprintCrowdAdmin",
                Name = "Mikael"
                };

                context.AddRange(user);
                context.SaveChanges();
            }
        }
    }
}