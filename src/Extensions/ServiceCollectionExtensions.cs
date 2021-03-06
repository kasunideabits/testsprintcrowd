namespace SprintCrowd.BackEnd.Extensions
{
    using System;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Protocols;
    using Microsoft.IdentityModel.Tokens;
    using SprintCrowd.BackEnd.CustomPolicies;
    using SprintCrowd.BackEnd.Enums;
    using SprintCrowd.BackEnd.Models;

    /// <summary>
    /// service collection extensions, used to authentication.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add jwt authentication.
        /// </summary>
        /// <param name="services">extension for ServiceCollection.</param>
        /// <param name="appSettings">application settings.</param>
        public static void AddSprintCrowdAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var httpDocumentRetriever = new HttpDocumentRetriever();
            httpDocumentRetriever.RequireHttps = false;
            Console.WriteLine($"{appSettings.AuthorizationServer + "/" + appSettings.OpenidConfigurationEndPoint}");
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                appSettings.AuthorizationServer + "/" + appSettings.OpenidConfigurationEndPoint,
                new OpenIdConnectConfigurationRetriever(),
                httpDocumentRetriever);
            var discoveryDocument = configurationManager.GetConfigurationAsync().Result;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                    // Clock skew compensates for server time drift.
                    // We recommend 5 minutes or less:
                    ClockSkew = TimeSpan.FromMinutes(5),
                    IssuerSigningKeys = discoveryDocument.SigningKeys,
                    RequireSignedTokens = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = discoveryDocument.Issuer,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    };
                    options.Authority = appSettings.AuthorizationServer;

                    // Must remove this when we go to production
                    options.RequireHttpsMetadata = false;
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.ADMIN, policy => policy.Requirements.Add(new HasScopeRequirement("scrowd-cp", discoveryDocument.Issuer)));
            });
        }
    }
}