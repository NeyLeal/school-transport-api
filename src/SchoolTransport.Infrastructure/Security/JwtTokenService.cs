using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Security;

public class JwtTokenService(IOptions<JwtSettings> options) : ITokenService
{
    private readonly JwtSettings _settings = options.Value;

    public TokenGerado Gerar(Usuario usuario, Guid? motoristaId, Guid? responsavelId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, usuario.Perfil.ToString())
        };

        if (motoristaId.HasValue)
            claims.Add(new Claim(ClaimsPersonalizadas.MotoristaId, motoristaId.Value.ToString()));

        if (responsavelId.HasValue)
            claims.Add(new Claim(ClaimsPersonalizadas.ResponsavelId, responsavelId.Value.ToString()));

        var expiraEm = DateTime.UtcNow.AddHours(_settings.ExpiracaoHoras);
        var credenciais = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expiraEm,
            signingCredentials: credenciais);

        return new TokenGerado(new JwtSecurityTokenHandler().WriteToken(token), expiraEm);
    }
}
