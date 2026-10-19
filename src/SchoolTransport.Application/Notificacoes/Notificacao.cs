namespace SchoolTransport.Application.Notificacoes;

/// <param name="UsuarioId">Usuário que recebe o push; nulo quando o destinatário ainda não tem acesso ao app.</param>
public record Notificacao(Guid? UsuarioId, string Destinatario, string Telefone, string Email, string Titulo, string Mensagem);

public interface INotificador
{
    Task EnviarAsync(Notificacao notificacao, CancellationToken cancellationToken = default);
}
