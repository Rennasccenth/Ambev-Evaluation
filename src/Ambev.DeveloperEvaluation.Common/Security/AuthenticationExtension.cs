using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ambev.DeveloperEvaluation.Common.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Common.Security;

public static class AuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.RegisterOption<JwtSettings>(JwtSettings.SectionName);
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(authenticationOptions =>
        {
            authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddOptions<JwtBearerOptions>()
            .Configure<IOptions<JwtSettings>, IHostEnvironment>((jwtOptions, jwtSettings, hostEnvironment) =>
            {
                JwtSettings settings = jwtSettings.Value;
                byte[] key = Encoding.UTF8.GetBytes(settings.SecretKey);

                if (hostEnvironment.IsProduction())
                {
                    jwtOptions.RequireHttpsMetadata = true;
                }
                jwtOptions.RequireHttpsMetadata = false;
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudiences = settings.Audiences,
                    ValidIssuer = settings.Issuer
                };
            });

        return services;
    }
}