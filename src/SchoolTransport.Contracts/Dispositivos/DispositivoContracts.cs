namespace SchoolTransport.Contracts.Dispositivos;

public class RegistrarDispositivoRequest
{
    public string Token { get; set; } = string.Empty;
    public string Plataforma { get; set; } = string.Empty;
}

public class RemoverDispositivoRequest
{
    public string Token { get; set; } = string.Empty;
}

public static class CanaisNotificacao
{
    public const string Viagens = "viagens";
}
