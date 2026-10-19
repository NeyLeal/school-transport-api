using SchoolTransport.Domain.Entities;

namespace SchoolTransport.Application.Common;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> EmailEmUsoAsync(string email, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default);
    void Remover(Usuario usuario);
}

public interface IMotoristaRepository
{
    Task<Motorista?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Motorista?> ObterPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<Motorista?> ObterPorUsuarioDoAjudanteAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Motorista motorista, CancellationToken cancellationToken = default);
}

public interface IEscolaRepository
{
    Task<List<Escola>> ListarAsync(Guid motoristaId, CancellationToken cancellationToken = default);
    Task<Escola?> ObterAsync(Guid motoristaId, Guid id, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Escola escola, CancellationToken cancellationToken = default);
}

public interface IResponsavelRepository
{
    Task<List<Responsavel>> ListarAsync(Guid motoristaId, CancellationToken cancellationToken = default);
    Task<Responsavel?> ObterAsync(Guid motoristaId, Guid id, CancellationToken cancellationToken = default);
    Task<Responsavel?> ObterPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Responsavel responsavel, CancellationToken cancellationToken = default);
}

public interface IAlunoRepository
{
    Task<List<Aluno>> ListarAsync(Guid motoristaId, bool incluirInativos, CancellationToken cancellationToken = default);
    Task<List<Aluno>> ListarPorResponsavelAsync(Guid responsavelId, CancellationToken cancellationToken = default);
    Task<Aluno?> ObterAsync(Guid id, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Aluno aluno, CancellationToken cancellationToken = default);
}

public interface IDispositivoPushRepository
{
    Task<DispositivoPush?> ObterPorTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<List<DispositivoPush>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(DispositivoPush dispositivo, CancellationToken cancellationToken = default);
    void Remover(DispositivoPush dispositivo);
}

public interface IViagemRepository
{
    Task<List<ViagemAluno>> ListarPorDataAsync(Guid motoristaId, DateOnly data, CancellationToken cancellationToken = default);
    Task<List<ViagemAluno>> ListarPorAlunosAsync(IEnumerable<Guid> alunoIds, DateOnly data, CancellationToken cancellationToken = default);
    Task<ViagemAluno?> ObterAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ViagemAluno?> ObterPorAlunoEDataAsync(Guid alunoId, DateOnly data, CancellationToken cancellationToken = default);
    Task<HashSet<Guid>> AlunosComViagemAsync(Guid motoristaId, DateOnly data, CancellationToken cancellationToken = default);
    Task AdicionarAsync(ViagemAluno viagem, CancellationToken cancellationToken = default);
    Task AdicionarVariasAsync(IEnumerable<ViagemAluno> viagens, CancellationToken cancellationToken = default);
}
