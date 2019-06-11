using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SprintCrowd.BackEnd;
using SprintCrowd.BackEnd.Application;
using SprintCrowd.BackEnd.Infrastructure.Persistence;
using SprintCrowd.BackEnd.Infrastructure.Persistence.Entities;
using SprintCrowd.BackEnd.Models;
using SprintCrowd.BackEnd.Web;

namespace Tests
{
  public class TestStartUp : Startup
  {
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
      services.AddMvc();
    }

    public override void AddSwagger(IServiceCollection services)
    { }

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
      app.UseMvc();
    }

    public void AddUserToInMeoryDatabase(IApplicationBuilder app)
    {
      var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ScrowdDbContext>();

      const string userEmail = "testUser@test.com";
      User user = new User()
      {
        Id = 1,
        FacebookUserId = "1",
        UserType = (int)UserType.AdminUser,
        Email = userEmail,
        Name = "Test User"
      };

      context.AddRange(user);
      context.SaveChanges();
    }
  }
}