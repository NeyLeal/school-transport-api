using SchoolTransport.Domain.Common;
using SchoolTransport.Domain.Enums;

namespace SchoolTransport.Domain.Entities;

public class AlunoResponsavel : BaseEntity
{
    public Guid AlunoId { get; private set; }
    public Guid ResponsavelId { get; private set; }
    public Responsavel Responsavel { get; private set; } = null!;
    public ParentescoResponsavel Parentesco { get; private set; }
    public bool PodeAlterarEndereco { get; private set; }
    public bool RecebeNotificacoes { get; private set; }

    private AlunoResponsavel() { }

    internal AlunoResponsavel(
        Guid alunoId,
        Guid responsavelId,
        ParentescoResponsavel parentesco,
        bool podeAlterarEndereco,
        bool recebeNotificacoes)
    {
        AlunoId = Guard.Obrigatorio(alunoId, "Vínculo inválido: aluno não informado.");
        ResponsavelId = Guard.Obrigatorio(responsavelId, "Vínculo inválido: responsável não informado.");
        Parentesco = parentesco;
        PodeAlterarEndereco = podeAlterarEndereco;
        RecebeNotificacoes = recebeNotificacoes;
    }

    internal void AtualizarPermissoes(ParentescoResponsavel parentesco, bool podeAlterarEndereco, bool recebeNotificacoes)
    {
        Parentesco = parentesco;
        PodeAlterarEndereco = podeAlterarEndereco;
        RecebeNotificacoes = recebeNotificacoes;
        MarcarComoAtualizado();
    }
}
