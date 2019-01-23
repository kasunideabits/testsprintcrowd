namespace SprintCrowd.Backend
{
    using System.Buffers;
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    using SprintCrowd.Backend.ExceptionHandler;
    using SprintCrowd.Backend.Interfaces;
    using SprintCrowd.Backend.Logger;
    using SprintCrowd.Backend.Models;
    using SprintCrowd.Backend.Persistence;
    using SprintCrowd.Backend.Repositories;
    using SprintCrowd.Backend.Services;
    using System.Reflection;
    using System.IO;
    using System;

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
            SLogger.appSettings = appSettings;
            SLogger.InitLogger();
            SLogger.Log("Init Logger.", LogType.Info);
            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,
                    ValidateLifetime = true
                };
            });
            services.AddDbContext<SprintCrowdDbContext>(options =>
                     options.UseNpgsql(this.Configuration.GetConnectionString("SprintCrowd")));

            services.AddMvc(options =>
            {
                //ignore self referencing loops newtonsoft.
                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }, ArrayPool<char>.Shared));
                options.Filters.Add<GlobalExceptionHandler>();
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
            app.UseMvc();
        }

        private void RegisterDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFacebookReporsitory, FacebookReporsitory>();
            services.AddScoped<IFacebookService, FacebookService>();
            // add userservice as dependecy injection
            services.AddScoped<IUserService, UserService>();
            SLogger.Log("Dependency injection registered.", LogType.Info);
        }
    }
}
