using SchoolTransport.Domain.Common;
using SchoolTransport.Domain.Enums;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.Domain.Entities;

public class Aluno : AggregateRoot
{
    public Guid MotoristaId { get; private set; }
    public Guid EscolaId { get; private set; }
    public Escola Escola { get; private set; } = null!;
    public string Nome { get; private set; } = string.Empty;
    public DateOnly? DataNascimento { get; private set; }
    public decimal ValorMensalidade { get; private set; }
    public string EnderecoPadraoIda { get; private set; } = string.Empty;
    public string EnderecoPadraoVolta { get; private set; } = string.Empty;
    public bool Ativo { get; private set; }

    private readonly List<AlunoResponsavel> _responsaveis = [];
    public IReadOnlyCollection<AlunoResponsavel> Responsaveis => _responsaveis.AsReadOnly();

    private Aluno() { }

    public Aluno(
        Guid motoristaId,
        Guid escolaId,
        string nome,
        DateOnly? dataNascimento,
        decimal valorMensalidade,
        string enderecoPadraoIda,
        string enderecoPadraoVolta)
    {
        MotoristaId = Guard.Obrigatorio(motoristaId, "O aluno precisa estar vinculado a um motorista.");
        DefinirDados(escolaId, nome, dataNascimento, valorMensalidade, enderecoPadraoIda, enderecoPadraoVolta);
        Ativo = true;
    }

    public void Atualizar(
        Guid escolaId,
        string nome,
        DateOnly? dataNascimento,
        decimal valorMensalidade,
        string enderecoPadraoIda,
        string enderecoPadraoVolta)
    {
        DefinirDados(escolaId, nome, dataNascimento, valorMensalidade, enderecoPadraoIda, enderecoPadraoVolta);
        MarcarComoAtualizado();
    }

    public void Desativar()
    {
        Ativo = false;
        MarcarComoAtualizado();
    }

    public void Reativar()
    {
        Ativo = true;
        MarcarComoAtualizado();
    }

    public string ObterEnderecoPadrao(Trajeto trajeto) =>
        trajeto == Trajeto.Ida ? EnderecoPadraoIda : EnderecoPadraoVolta;

    public AlunoResponsavel VincularResponsavel(
        Guid responsavelId,
        ParentescoResponsavel parentesco,
        bool podeAlterarEndereco = true,
        bool recebeNotificacoes = true)
    {
        if (_responsaveis.Any(r => r.ResponsavelId == responsavelId))
            throw new DomainException("Este responsável já está vinculado a este aluno.");

        var vinculo = new AlunoResponsavel(Id, responsavelId, parentesco, podeAlterarEndereco, recebeNotificacoes);
        _responsaveis.Add(vinculo);
        MarcarComoAtualizado();
        return vinculo;
    }

    public void AtualizarVinculoResponsavel(
        Guid responsavelId,
        ParentescoResponsavel parentesco,
        bool podeAlterarEndereco,
        bool recebeNotificacoes)
    {
        ObterVinculo(responsavelId).AtualizarPermissoes(parentesco, podeAlterarEndereco, recebeNotificacoes);
        MarcarComoAtualizado();
    }

    public void DesvincularResponsavel(Guid responsavelId)
    {
        _responsaveis.Remove(ObterVinculo(responsavelId));
        MarcarComoAtualizado();
    }

    public void GarantirResponsavelVinculado(Guid responsavelId)
    {
        ObterVinculo(responsavelId);
    }

    public void GarantirPermissaoAlteracaoEndereco(Guid responsavelId)
    {
        if (!ObterVinculo(responsavelId).PodeAlterarEndereco)
            throw new DomainException("Este responsável não tem permissão para alterar o endereço. É necessário acordo direto com o motorista.");
    }

    private AlunoResponsavel ObterVinculo(Guid responsavelId) =>
        _responsaveis.FirstOrDefault(r => r.ResponsavelId == responsavelId)
            ?? throw new DomainException("Este responsável não está vinculado a este aluno.");

    private void DefinirDados(
        Guid escolaId,
        string nome,
        DateOnly? dataNascimento,
        decimal valorMensalidade,
        string enderecoPadraoIda,
        string enderecoPadraoVolta)
    {
        if (valorMensalidade <= 0)
            throw new DomainException("O valor da mensalidade deve ser maior que zero.");

        EscolaId = Guard.Obrigatorio(escolaId, "O aluno precisa estar vinculado a uma escola.");
        Nome = Guard.Obrigatorio(nome, "O nome do aluno é obrigatório.");
        DataNascimento = dataNascimento;
        ValorMensalidade = valorMensalidade;
        EnderecoPadraoIda = Guard.Obrigatorio(enderecoPadraoIda, "O endereço de ida é obrigatório.");
        EnderecoPadraoVolta = Guard.Obrigatorio(enderecoPadraoVolta, "O endereço de volta é obrigatório.");
    }
}
