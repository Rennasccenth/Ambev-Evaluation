using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ambev.DeveloperEvaluation.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Common.Security;

public static class AuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOption<JwtSettings>(JwtSettings.SectionName);
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(authenticationOptions =>
        {
            authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            JwtSettings jwtSettings = new() { SecretKey = null!, Issuer = null!, Audiences = [] };
            configuration.GetRequiredSection(JwtSettings.SectionName).Bind(jwtSettings);

            byte[] key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudiences = jwtSettings.Audiences,
                ValidIssuer = jwtSettings.Issuer
            };
        });

        return services;
    }
}