namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    using Hangfire.PostgreSql;
    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Hangfire configuration class
    /// </summary>
    public static class NotificationWorkerEntry
    {
        /// <summary>
        /// Initialize hangfire
        /// </summary>
        public static void Initialize(IConfiguration config, IServiceCollection services)
        {
            var notificationWorkerSecsion = config.GetSection("NotificationConfig");
            var notificationWorkerConfig = notificationWorkerSecsion.Get<NotificationWorkerConfig>();
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(notificationWorkerConfig.HangfireConnection));
            services.AddHangfireServer();
        }

        /// <summary>
        /// Enable dashboard
        /// </summary>
        public static void EnableWorkerDashboard(IApplicationBuilder app)
        {
           var dashboardOptions =
            new DashboardOptions
            {
                IgnoreAntiforgeryToken = true
            };

            app.UseHangfireDashboard("/hangfire", dashboardOptions);
        }
    }
}