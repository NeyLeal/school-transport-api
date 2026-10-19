using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using SchoolTransport.Application.Common;
using SchoolTransport.Application.Notificacoes;
using SchoolTransport.Contracts.Dispositivos;

namespace SchoolTransport.Infrastructure.Servicos;

public class FcmNotificador(
    FirebaseMessaging messaging,
    IDispositivoPushRepository dispositivos,
    ILogger<FcmNotificador> logger) : INotificador
{
    private static readonly HashSet<MessagingErrorCode> ErrosDeTokenInvalido =
        [MessagingErrorCode.Unregistered, MessagingErrorCode.InvalidArgument, MessagingErrorCode.SenderIdMismatch];

    public async Task EnviarAsync(Notificacao notificacao, CancellationToken cancellationToken = default)
    {
        if (notificacao.UsuarioId is not { } usuarioId)
        {
            logger.LogInformation("{Destinatario} não tem acesso ao app; push não enviado: {Mensagem}", notificacao.Destinatario, notificacao.Mensagem);
            return;
        }

        var aparelhos = await dispositivos.ListarPorUsuarioAsync(usuarioId, cancellationToken);

        if (aparelhos.Count == 0)
        {
            logger.LogInformation("{Destinatario} não tem aparelho registrado; push não enviado: {Mensagem}", notificacao.Destinatario, notificacao.Mensagem);
            return;
        }

        var mensagem = new MulticastMessage
        {
            // O app ainda registra tokens de FCM; migrar para Fids quando o cliente Android expuser o ID de instalação.
#pragma warning disable CS0618
            Tokens = aparelhos.Select(a => a.Token).ToList(),
#pragma warning restore CS0618
            Notification = new Notification { Title = notificacao.Titulo, Body = notificacao.Mensagem },
            Android = new AndroidConfig
            {
                Priority = Priority.High,
                // Sem EventTimestamp o SDK envia DateTime.MinValue e o Android mostra "há 2032 anos".
                Notification = new AndroidNotification { ChannelId = CanaisNotificacao.Viagens, EventTimestamp = DateTime.UtcNow }
            }
        };

        // Falha de rede/credencial lança e o outbox tenta de novo; falhas por aparelho são tratadas abaixo.
        var resposta = await messaging.SendEachForMulticastAsync(mensagem, cancellationToken);

        for (var i = 0; i < resposta.Responses.Count; i++)
        {
            var resultado = resposta.Responses[i];

            if (resultado.IsSuccess)
                continue;

            if (resultado.Exception?.MessagingErrorCode is { } codigo && ErrosDeTokenInvalido.Contains(codigo))
            {
                // Removido junto com o SaveChanges do lote do outbox.
                dispositivos.Remover(aparelhos[i]);
                logger.LogInformation("Token de push inválido removido para {Destinatario} ({Codigo})", notificacao.Destinatario, codigo);
            }
            else
            {
                logger.LogWarning(resultado.Exception, "Falha ao enviar push para {Destinatario}", notificacao.Destinatario);
            }
        }

        logger.LogInformation("Push para {Destinatario}: {Sucesso} de {Total} aparelhos", notificacao.Destinatario, resposta.SuccessCount, aparelhos.Count);
    }
}
