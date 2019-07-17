﻿namespace SprintCrowd.BackEnd
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
    using SprintCrowd.BackEnd.Data;
    using SprintCrowd.BackEnd.Domain.Device;
    using SprintCrowd.BackEnd.Domain.ScrowdUser;
    using SprintCrowd.BackEnd.Domain.Sprint;
    using SprintCrowd.BackEnd.Extensions;
    using SprintCrowd.BackEnd.Infrastructure.Notifier;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
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
                c.RoutePrefix = string.Empty;
            });
            app.UseAuthentication();
            app.UseSwagger();
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
            services.AddSingleton<INotifyFactory, NotifyFactory>();
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