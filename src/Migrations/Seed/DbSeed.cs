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
            const string _systemUserEmail = "system@sprintcrowd.se";
            const string _adminUserEmail = "admin@sprintcrowd.se";
            var context = services.GetRequiredService<ScrowdDbContext>();

            var admin = context.User.Where(s => s.Email == _adminUserEmail).FirstOrDefault();

            if (admin == null)
            {
                User user = new User()
                {
                UserType = (int)UserType.AdminUser,
                Email = _adminUserEmail,
                FacebookUserId = "SprintCrowdAdmin",
                Name = "Mikael",
                ColorCode = new UserColorCode().PickColor(),
                Code = SCrowdUniqueKey.GetUniqueKey()
                };

                context.AddRange(user);
            }

            var systemUser = context.User.Where(s => s.Email == _systemUserEmail).FirstOrDefault();
            if (systemUser == null)
            {
                User user = new User()
                {
                UserType = (int)UserType.SystemUser,
                Email = _systemUserEmail,
                FacebookUserId = "SprintCrowdAdmin",
                Name = "System User",
                ColorCode = new UserColorCode().PickColor(),
                Code = SCrowdUniqueKey.GetUniqueKey()
                };

                context.AddRange(user);
            }
            context.SaveChanges();
        }
    }
}