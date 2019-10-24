namespace Tests
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SprintCrowd.BackEnd.Application;
    using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
    using SprintCrowd.BackEnd.Infrastructure.Persistence;
    using SprintCrowd.BackEnd.Models;
    using SprintCrowd.BackEnd.Web;
    using SprintCrowd.BackEnd;
    using Tests.Mocks;

    public class TestStartUp : Startup
    {
        public static ScrowdDbContext DbContext;
        public TestStartUp(IConfiguration configuration) : base(configuration)
        {

        }

        public override void AddAuthentication(IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme"; // has to match scheme in TestAuthenticationExtensions
                options.DefaultChallengeScheme = "Test Scheme";
            }).AddTestAuth(o => { });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(SprintCrowd.BackEnd.Enums.Policy.ADMIN, policy => policy.Requirements.Add(new HasScopeRequirement()));
            });
            services.AddMvc();
        }

        public override void AddSwagger(IServiceCollection services) { }

        public override void AddDatabase(IServiceCollection services)
        {
            services.AddDbContext<ScrowdDbContext>(options =>
                options.UseInMemoryDatabase("InMemory"));
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            app.UseAuthentication();
            // app.UseSwaggerUI(c =>
            // {
            //   c.SwaggerEndpoint("/swagger/v1/swagger.json", "SprintCrowd API");
            //   c.RoutePrefix = string.Empty;
            // });
            app.UseAuthentication();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            this.AddUserToInMemoryDatabase(app);
            app.UseMvc();
        }

        public void AddUserToInMemoryDatabase(IApplicationBuilder app)
        {
            TestStartUp.DbContext = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ScrowdDbContext>();
            const string userEmail = "testUser@test.com";
            User user = new User()
            {
                Id = 1,
                FacebookUserId = "1",
                UserType = (int)UserType.AdminUser,
                Email = userEmail,
                Name = "Test User"
            };

            TestStartUp.DbContext.AddRange(user);
            TestStartUp.DbContext.SaveChanges();
        }

        /// <summary>
        /// adds authoization modules
        /// </summary>
        /// <param name="services"></param>
        public override void AddAuthorizationDIModules(IServiceCollection services)
        {
            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }
    }
}