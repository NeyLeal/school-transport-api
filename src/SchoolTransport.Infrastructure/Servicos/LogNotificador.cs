using Microsoft.Extensions.Logging;
using SchoolTransport.Application.Notificacoes;

namespace SchoolTransport.Infrastructure.Servicos;

public class LogNotificador(ILogger<LogNotificador> logger) : INotificador
{
    public Task EnviarAsync(Notificacao notificacao, CancellationToken cancellationToken = default)
    {
        logger.LogInformation(
            "Notificação para {Destinatario} ({Telefone}): {Titulo} - {Mensagem}",
            notificacao.Destinatario,
            notificacao.Telefone,
            notificacao.Titulo,
            notificacao.Mensagem);

        return Task.CompletedTask;
    }
}
