using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ambev.DeveloperEvaluation.Common.Options;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Common.Security;

public static class AuthenticationExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.RegisterOption<JwtSettings>(JwtSettings.SectionName);
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        // TODO: WIP testing it later. The project was broken in few places before reach this path.
        // var secretKey = configuration["Jwt:SecretKey"];
        // ArgumentException.ThrowIfNullOrWhiteSpace(secretKey);
        //
        // var key = Encoding.ASCII.GetBytes(secretKey);
        //
        // services
        //     .AddAuthentication(x =>
        //     {
        //         x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //         x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //     })
        //     .AddJwtBearer(x =>
        //     {
        //         x.RequireHttpsMetadata = false;
        //         x.SaveToken = true;
        //         x.TokenValidationParameters = new TokenValidationParameters
        //         {
        //             ValidateIssuerSigningKey = true,
        //             IssuerSigningKey = new SymmetricSecurityKey(key),
        //             ValidateIssuer = false,
        //             ValidateAudience = false,
        //             ClockSkew = TimeSpan.Zero
        //         };
        //     });

        services.AddAuthentication(authenticationOptions =>
        {
            authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddOptions<JwtBearerOptions>()
            .Configure<IOptions<JwtSettings>>((jwtOptions, jwtSettings) =>
            {
                JwtSettings settings = jwtSettings.Value;

                byte[] key = Encoding.UTF8.GetBytes(settings.SecretKey);

                jwtOptions.RequireHttpsMetadata = settings.RequireMetadata;
                jwtOptions.SaveToken = settings.SaveToken;
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}