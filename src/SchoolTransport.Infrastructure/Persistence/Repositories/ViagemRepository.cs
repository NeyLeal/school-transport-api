using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Repositories;

public class ViagemRepository(SchoolTransportDbContext context) : IViagemRepository
{
    private IQueryable<ViagemAluno> ComAluno() =>
        context.Viagens.Include(x => x.Aluno).ThenInclude(a => a.Escola);

    public Task<List<ViagemAluno>> ListarPorDataAsync(Guid motoristaId, DateOnly data, CancellationToken cancellationToken = default) =>
        ComAluno()
            .Where(x => x.MotoristaId == motoristaId && x.Data == data)
            .OrderBy(x => x.Aluno.Escola.Nome)
            .ThenBy(x => x.Aluno.Nome)
            .ToListAsync(cancellationToken);

    public Task<List<ViagemAluno>> ListarPorAlunosAsync(IEnumerable<Guid> alunoIds, DateOnly data, CancellationToken cancellationToken = default)
    {
        var ids = alunoIds.Distinct().ToList();
        return ComAluno()
            .Where(x => ids.Contains(x.AlunoId) && x.Data == data)
            .ToListAsync(cancellationToken);
    }

    public Task<ViagemAluno?> ObterAsync(Guid id, CancellationToken cancellationToken = default) =>
        ComAluno().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<ViagemAluno?> ObterPorAlunoEDataAsync(Guid alunoId, DateOnly data, CancellationToken cancellationToken = default) =>
        ComAluno().FirstOrDefaultAsync(x => x.AlunoId == alunoId && x.Data == data, cancellationToken);

    public async Task<HashSet<Guid>> AlunosComViagemAsync(Guid motoristaId, DateOnly data, CancellationToken cancellationToken = default)
    {
        var ids = await context.Viagens
            .Where(x => x.MotoristaId == motoristaId && x.Data == data)
            .Select(x => x.AlunoId)
            .ToListAsync(cancellationToken);

        return [.. ids];
    }

    public async Task AdicionarAsync(ViagemAluno viagem, CancellationToken cancellationToken = default) =>
        await context.Viagens.AddAsync(viagem, cancellationToken);

    public async Task AdicionarVariasAsync(IEnumerable<ViagemAluno> viagens, CancellationToken cancellationToken = default) =>
        await context.Viagens.AddRangeAsync(viagens, cancellationToken);
}
