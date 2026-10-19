using SchoolTransport.Domain.Common;

namespace SchoolTransport.Domain.Entities;

public class DispositivoPush : AggregateRoot
{
    public Guid UsuarioId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public string Plataforma { get; private set; } = string.Empty;

    private DispositivoPush() { }

    public DispositivoPush(Guid usuarioId, string token, string plataforma)
    {
        UsuarioId = Guard.Obrigatorio(usuarioId, "O dispositivo precisa estar vinculado a um usuário.");
        Token = Guard.Obrigatorio(token, "O token do dispositivo é obrigatório.");
        Plataforma = Guard.Obrigatorio(plataforma, "A plataforma do dispositivo é obrigatória.");
    }

    public void VincularA(Guid usuarioId, string plataforma)
    {
        UsuarioId = Guard.Obrigatorio(usuarioId, "O dispositivo precisa estar vinculado a um usuário.");
        Plataforma = Guard.Obrigatorio(plataforma, "A plataforma do dispositivo é obrigatória.");
        MarcarComoAtualizado();
    }
}
