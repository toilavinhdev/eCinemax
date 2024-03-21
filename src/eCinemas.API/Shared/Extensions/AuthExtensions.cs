using System.Text;
using eCinemas.API.Shared.ValueObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace eCinemas.API.Shared.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtBearerAuth(this IServiceCollection services, 
                                                      JwtConfig config, 
                                                      Action<JwtBearerOptions>? jwtOptions = null)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                {
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.TokenSingingKey));
                    
                    // defaults
                    options.TokenValidationParameters.IssuerSigningKey = key;
                    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    options.TokenValidationParameters.ValidateLifetime = true;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(60);
                    options.TokenValidationParameters.ValidAudience = null;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ValidIssuer = null;
                    options.TokenValidationParameters.ValidateIssuer = false;
                    
                    jwtOptions?.Invoke(options);

                    // correct any user mistake
                    options.TokenValidationParameters.ValidateAudience = options.TokenValidationParameters.ValidAudience is not null;
                    options.TokenValidationParameters.ValidateIssuer = options.TokenValidationParameters.ValidIssuer is not null;
                    
                    // events
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = _ => throw new UnauthorizedAccessException(),
                        OnAuthenticationFailed = _ => throw new UnauthorizedAccessException(),
                        OnChallenge = _ => throw new UnauthorizedAccessException(),
                    };
                });
        
        return services;
    }
}