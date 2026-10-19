using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Infrastructure.Persistence.Repositories;

public class AlunoRepository(SchoolTransportDbContext context) : IAlunoRepository
{
    public Task<List<Aluno>> ListarAsync(Guid motoristaId, bool incluirInativos, CancellationToken cancellationToken = default) =>
        context.Alunos
            .Include(x => x.Escola)
            .Where(x => x.MotoristaId == motoristaId && (incluirInativos || x.Ativo))
            .OrderBy(x => x.Nome)
            .ToListAsync(cancellationToken);

    public Task<List<Aluno>> ListarPorResponsavelAsync(Guid responsavelId, CancellationToken cancellationToken = default) =>
        context.Alunos
            .Include(x => x.Escola)
            .Include(x => x.Responsaveis)
            .Where(x => x.Ativo && x.Responsaveis.Any(r => r.ResponsavelId == responsavelId))
            .OrderBy(x => x.Nome)
            .ToListAsync(cancellationToken);

    public Task<Aluno?> ObterAsync(Guid id, CancellationToken cancellationToken = default) =>
        context.Alunos
            .Include(x => x.Escola)
            .Include(x => x.Responsaveis)
                .ThenInclude(r => r.Responsavel)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AdicionarAsync(Aluno aluno, CancellationToken cancellationToken = default) =>
        await context.Alunos.AddAsync(aluno, cancellationToken);
}
