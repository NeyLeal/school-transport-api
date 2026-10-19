using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolTransport.Application.Notificacoes;
using SchoolTransport.Domain.Events;
using SchoolTransport.Infrastructure.Persistence;

namespace SchoolTransport.Infrastructure.Outbox;

public class OutboxProcessor(
    SchoolTransportDbContext context,
    NotificacaoViagemService notificacoes,
    ILogger<OutboxProcessor> logger)
{
    private const int TamanhoLote = 20;

    private static readonly Dictionary<string, Type> TiposDeEvento = typeof(IDomainEvent).Assembly
        .GetTypes()
        .Where(t => typeof(IDomainEvent).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
        .ToDictionary(t => t.Name);

    public async Task<int> ProcessarAsync(CancellationToken cancellationToken = default)
    {
        var mensagens = await context.OutboxMessages
            .Where(m => m.ProcessadoEm == null)
            .OrderBy(m => m.OcorridoEm)
            .Take(TamanhoLote)
            .ToListAsync(cancellationToken);

        foreach (var mensagem in mensagens)
        {
            try
            {
                if (TiposDeEvento.TryGetValue(mensagem.Tipo, out var tipo)
                    && JsonSerializer.Deserialize(mensagem.Conteudo, tipo) is IDomainEvent evento)
                {
                    await notificacoes.ProcessarAsync(evento, cancellationToken);
                }

                mensagem.MarcarProcessada();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao processar a mensagem {MensagemId} do outbox", mensagem.Id);
                mensagem.RegistrarFalha(ex.Message);
            }
        }

        if (mensagens.Count > 0)
            await context.SaveChangesAsync(cancellationToken);

        return mensagens.Count;
    }
}
