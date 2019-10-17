namespace SprintCrowd.BackEnd.Infrastructure.NotificationWorker
{
    using Hangfire.PostgreSql;
    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class NotificationWorkerEntry
    {
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

        public static void EnableWorkerDashboard(IApplicationBuilder app)
        {
            app.UseHangfireDashboard();
        }
    }
}