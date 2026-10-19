using SchoolTransport.Infrastructure.Outbox;

namespace SchoolTransport.Worker;

public class OutboxWorker(IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<OutboxWorker> logger) : BackgroundService
{
    private readonly TimeSpan _intervalo = TimeSpan.FromSeconds(configuration.GetValue("Outbox:IntervaloSegundos", 5));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var processadas = await scope.ServiceProvider.GetRequiredService<OutboxProcessor>().ProcessarAsync(stoppingToken);

                if (processadas > 0)
                    logger.LogInformation("{Quantidade} mensagens do outbox processadas", processadas);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Erro ao processar o outbox");
            }

            await Task.Delay(_intervalo, stoppingToken);
        }
    }
}
