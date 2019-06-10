namespace backend.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using SprintCrowdBackEnd.Application;
    using SprintCrowdBackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowdBackEnd.Infrastructure.Persistence;

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
                Id = 1,
                UserType = Convert.ToInt32(UserType.AdmiUser),
                Email = adminUserEmail,
                Name = "Mikael"
                };

                context.AddRange(user);
                context.SaveChanges();
            }

        }

    }

}