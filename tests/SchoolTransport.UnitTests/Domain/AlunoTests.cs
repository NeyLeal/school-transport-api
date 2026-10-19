using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.UnitTests.Domain;

public class AlunoTests
{
    private static Aluno NovoAluno() =>
        new(Guid.NewGuid(), Guid.NewGuid(), "Ana", null, 350m, "Rua A, 1", "Rua A, 1");

    [Fact]
    public void Nao_deve_criar_aluno_com_mensalidade_invalida()
    {
        Assert.Throws<DomainException>(() =>
            new Aluno(Guid.NewGuid(), Guid.NewGuid(), "Ana", null, 0m, "Rua A, 1", "Rua A, 1"));
    }

    [Fact]
    public void Nao_deve_vincular_mesmo_responsavel_duas_vezes()
    {
        var aluno = NovoAluno();
        var responsavelId = Guid.NewGuid();

        aluno.VincularResponsavel(responsavelId, ParentescoResponsavel.Mae);

        Assert.Throws<DomainException>(() => aluno.VincularResponsavel(responsavelId, ParentescoResponsavel.Mae));
    }

    [Fact]
    public void Responsavel_sem_permissao_nao_altera_endereco()
    {
        var aluno = NovoAluno();
        var responsavelId = Guid.NewGuid();
        aluno.VincularResponsavel(responsavelId, ParentescoResponsavel.Pai, podeAlterarEndereco: false);

        Assert.Throws<DomainException>(() => aluno.GarantirPermissaoAlteracaoEndereco(responsavelId));
    }

    [Fact]
    public void Responsavel_nao_vinculado_e_rejeitado()
    {
        var aluno = NovoAluno();

        Assert.Throws<DomainException>(() => aluno.GarantirResponsavelVinculado(Guid.NewGuid()));
    }

    [Fact]
    public void Deve_desvincular_responsavel()
    {
        var aluno = NovoAluno();
        var responsavelId = Guid.NewGuid();
        aluno.VincularResponsavel(responsavelId, ParentescoResponsavel.Avo);

        aluno.DesvincularResponsavel(responsavelId);

        Assert.Empty(aluno.Responsaveis);
    }
}
