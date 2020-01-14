namespace SprintCrowd.BackEnd
{
    using System.Buffers;
    using System.IO;
    using System.Reflection;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using RestSharp;
    using SprintCrowd.BackEnd.CustomPolicies;
    using SprintCrowd.BackEnd.Domain.Achievement;
    using SprintCrowd.BackEnd.Domain.Admin.Dashboard;
    using SprintCrowd.BackEnd.Domain.Crons;
    using SprintCrowd.BackEnd.Domain.Device;
    using SprintCrowd.BackEnd.Domain.Friend;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Domain.SprintParticipant;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.NotificationWorker;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Infrastructure.PushNotification;
    using SprintCrowd.BackEnd.Infrastructure.RealTimeMessage;
    using SprintCrowd.BackEnd.Migrations.Seed;
    using SprintCrowd.BackEnd.Models;
    using SprintCrowd.BackEnd.Web;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// start class for the dotnet core application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// just a reference to Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">generated automatically</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">generated automatically</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            // configure strongly typed settings objects
            var appSettingsSection = this.Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var notificationWorkerConfigSection = this.Configuration.GetSection("NotificationConfig");
            services.Configure<NotificationWorkerConfig>(notificationWorkerConfigSection);

            var firebaseConfigSection = this.Configuration.GetSection("FirebaseConfig");
            services.Configure<FirebaseConfig>(firebaseConfigSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            this.AddAuthentication(services, appSettings);
            this.AddDatabase(services);
            services.AddMvc(options =>
            {
                // ignore self referencing loops newtonsoft.
                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new JsonOutputFormatter(
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    }, ArrayPool<char>.Shared));
            });
            this.AddSwagger(services);
            this.RegisterDependencyInjection(services);
            NotificationWorkerEntry.Initialize(this.Configuration, services);
        }

        /// <summary>
        /// Adds jwt token authentication
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSettings"></param>
        public virtual void AddAuthentication(IServiceCollection services, AppSettings appSettings)
        {
            services.AddSprintCrowdAuthentication(appSettings);
        }

        /// <summary>
        /// Adds db context
        /// </summary>
        /// <param name="services"></param>
        public virtual void AddDatabase(IServiceCollection services)
        {
            services.AddDbContext<ScrowdDbContext>(options =>
                options.UseNpgsql(this.Configuration.GetConnectionString("SprintCrowd")));
        }

        /// <summary>
        /// adds swagger
        /// </summary>
        /// <param name="services"></param>
        public virtual void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SprintCrowd API", Version = "v1" });
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">generated automatically</param>
        public virtual void Configure(IApplicationBuilder app)
        {
            NotificationWorkerEntry.EnableWorkerDashboard(app);

            app.UseStaticFiles();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseAuthentication();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SprintCrowd API");
            });

            app.UseAuthentication();
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Host = httpReq.Host.Value;;
                    swaggerDoc.BasePath = httpReq.PathBase;
                });
            });
            app.UseMiddleware<ErrorHandlingMiddleware>();
            DbSeed.InitializeData(app.ApplicationServices.CreateScope().ServiceProvider);
            app.UseMvc();
        }

        /// <summary>
        /// registers dependecy injections
        /// </summary>
        /// <param name="services">passed from ConfigureServices.</param>
        private void RegisterDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISprintRepo, SprintRepo>();
            services.AddScoped<ISprintService, SprintService>();
            services.AddScoped<IDeviceService, DeviceService>();
            services.AddScoped<IDeviceRepo, DeviceRepo>();
            services.AddScoped<ISprintParticipantRepo, SprintParticipantRepo>();
            services.AddScoped<ISprintParticipantService, SprintParticipantService>();
            services.AddTransient<IFriendService, FriendService>();
            services.AddTransient<IFriendRepo, FriendRepo>();
            services.AddSingleton<IAblyConnectionFactory, AblyConnectionFactory>();
            services.AddTransient<IResetUserCodeService, ResetUserCodeService>();
            services.AddTransient<IResetUserCodeRepo, ResetUserCodeRepo>();
            services.AddScoped<INotificationClient, NotificationClient>();
            services.AddScoped<IPushNotificationClient, PushNotificationClient>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IDashboardRepo, DashboardRepo>();
            services.AddTransient<IAchievementService, AchievementService>();
            services.AddTransient<IAchievementRepo, AchievementRepo>();

            this.AddAuthorizationDIModules(services);
        }

        /// <summary>
        /// adds authoization modules
        /// </summary>
        /// <param name="services"></param>
        public virtual void AddAuthorizationDIModules(IServiceCollection services)
        {
            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }
    }
}