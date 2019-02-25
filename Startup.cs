namespace SprintCrowd.Backend
{
    using System.Buffers;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Protocols;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using SprintCrowd.Backend.Application;
    using SprintCrowd.Backend.Enums;
    using SprintCrowd.Backend.Models;
    using SprintCrowd.Backend.Web;
    using SprintCrowdBackEnd.Domain.ScrowdUser;
    using SprintCrowdBackEnd.Extensions;
    using SprintCrowdBackEnd.Infrastructure.Persistence;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            // configure strongly typed settings objects
            // var appSettingsSection = this.Configuration.GetSection("AppSettings");
            // services.Configure<AppSettings>(appSettingsSection);
            // var appSettings = appSettingsSection.Get<AppSettings>();
            // Console.WriteLine(appSettings.OpenidConfigurationEndPoint);
            // services.AddSprintCrowdAuthentication(appSettings);
            // services.AddDbContext<ScrowdDbContext>(options =>
            //     options.UseNpgsql(this.Configuration.GetConnectionString("SprintCrowd")));
            // services.AddMvc(options =>
            // {
            //     //ignore self referencing loops newtonsoft.
            //     options.OutputFormatters.Clear();
            //     options.OutputFormatters.Add(new JsonOutputFormatter(
            //         new JsonSerializerSettings()
            //         {
            //             ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //         }, ArrayPool<char>.Shared));
            // });
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new Info { Title = "SprintCrowd API", Version = "v1" });
            //     string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //     string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //     c.IncludeXmlComments(xmlPath);
            // });
            // RegisterDependencyInjection(services);
            Console.ReadLine();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">generated automatically</param>
        public void Configure(IApplicationBuilder app)
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
            app.UseMvc();
        }

        /// <summary>
        /// registers dependecy injections
        /// </summary>
        /// <param name="services">passed from ConfigureServices.</param>
        private static void RegisterDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}