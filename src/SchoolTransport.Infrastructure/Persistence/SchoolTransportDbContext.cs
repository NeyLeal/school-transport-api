using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Common;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Infrastructure.Outbox;

namespace SchoolTransport.Infrastructure.Persistence;

public class SchoolTransportDbContext(DbContextOptions<SchoolTransportDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Motorista> Motoristas => Set<Motorista>();
    public DbSet<Ajudante> Ajudantes => Set<Ajudante>();
    public DbSet<Escola> Escolas => Set<Escola>();
    public DbSet<Responsavel> Responsaveis => Set<Responsavel>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<AlunoResponsavel> AlunoResponsaveis => Set<AlunoResponsavel>();
    public DbSet<ViagemAluno> Viagens => Set<ViagemAluno>();
    public DbSet<DispositivoPush> DispositivosPush => Set<DispositivoPush>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolTransportDbContext).Assembly);
    }

    public async Task SalvarAsync(CancellationToken cancellationToken = default)
    {
        await SaveChangesAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        RegistrarEventosNoOutbox();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void RegistrarEventosNoOutbox()
    {
        var agregados = ChangeTracker
            .Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .Where(e => e.Eventos.Count > 0)
            .ToList();

        foreach (var agregado in agregados)
        {
            foreach (var evento in agregado.Eventos)
            {
                OutboxMessages.Add(new OutboxMessage(
                    evento.GetType().Name,
                    JsonSerializer.Serialize(evento, evento.GetType()),
                    evento.OcorridoEm));
            }

            agregado.LimparEventos();
        }
    }
}
