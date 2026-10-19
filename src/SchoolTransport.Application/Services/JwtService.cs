using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using SchoolTransport.Application.DTOs.Auth;
using SchoolTransport.Application.Interfaces;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Services;

public class JwtService : IJwtService
{
    private readonly JwtSettings _settings;

    public JwtService(
        IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }

    public string GenerateToken(
        User user)
    {
        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new(
                ClaimTypes.Email,
                user.Email),

            new(
                ClaimTypes.Role,
                user.Role.ToString())
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _settings.Key));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer:
                    _settings.Issuer,

                audience:
                    _settings.Audience,

                claims:
                    claims,

                expires:
                    DateTime.UtcNow.AddDays(7),

                signingCredentials:
                    credentials
            );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}