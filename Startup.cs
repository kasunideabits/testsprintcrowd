namespace SprintCrowd.Backend
{
    using System.Buffers;
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore;
    using Swashbuckle.AspNetCore.Swagger;
    using SprintCrowd.Backend.Enums;
    using SprintCrowd.Backend.Models;
    using System.Reflection;
    using System.IO;
    using System;
    using SprintCrowd.Backend.Application;
    using SprintCrowd.Backend.Web;
    using SprintCrowdBackEnd.Infrastructure.Persistence;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddDbContext<ScrowdDbContext>(options =>
                     options.UseNpgsql(this.Configuration.GetConnectionString("SprintCrowd")));

            services.AddMvc(options =>
            {
                //ignore self referencing loops newtonsoft.
                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }, ArrayPool<char>.Shared));
            });
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "SprintCrowd API", Version = "v1" });
                    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }
            );
            RegisterDependencyInjection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseSwagger();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();
        }

        private static void RegisterDependencyInjection(IServiceCollection services)
        {
            
        }
    }
}
