namespace SchoolTransport.Infrastructure.Security;

public class JwtSettings
{
    public const string Secao = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiracaoHoras { get; set; } = 168;
}
