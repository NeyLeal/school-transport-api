using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SchoolTransport.Infrastructure.Security;

namespace SchoolTransport.API.Extensions;

public static class AutenticacaoExtensions
{
    public static IServiceCollection AddAutenticacaoJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection(JwtSettings.Secao).Get<JwtSettings>()
            ?? throw new InvalidOperationException("Seção Jwt não configurada.");

        if (Encoding.UTF8.GetByteCount(jwt.Key) < 32)
            throw new InvalidOperationException("Jwt:Key deve ter pelo menos 32 bytes.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
                };
            });

        services.AddAuthorization();
        return services;
    }
}
