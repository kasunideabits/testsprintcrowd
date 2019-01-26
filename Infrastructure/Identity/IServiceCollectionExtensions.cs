namespace SprintCrowd.Backend.Infrastructure.Identity
{
    using System.Collections.Generic;
    using IdentityServer4.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;

    public static class IServiceCollectionExtensions
    {
        public static void AddSprintCrowdIdentity(this IServiceCollection services)
        {
            services.AddIdentityServer()
                    .AddInMemoryIdentityResources(GetIdentityResources())
                    .AddInMemoryClients(GetClients())
                    .AddInMemoryApiResources(GetApis())
                    .AddDeveloperSigningCredential();

            // TODO: this should be moved out.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = "http://localhost:5000";
                options.Audience = "scapi";
            });


        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource> {
                new ApiResource("scapi", "SprintCrowd")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "scapi" }
                }
            };
        }
    }
}