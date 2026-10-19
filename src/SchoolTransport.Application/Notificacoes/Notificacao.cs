namespace SchoolTransport.Application.Notificacoes;

public record Notificacao(string Destinatario, string Telefone, string Email, string Titulo, string Mensagem);

public interface INotificador
{
    Task EnviarAsync(Notificacao notificacao, CancellationToken cancellationToken = default);
}
