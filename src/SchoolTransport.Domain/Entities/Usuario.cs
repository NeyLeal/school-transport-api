using SchoolTransport.Domain.Common;
using SchoolTransport.Domain.Enums;

namespace SchoolTransport.Domain.Entities;

public class Usuario : AggregateRoot
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public PerfilUsuario Perfil { get; private set; }
    public bool Ativo { get; private set; }

    private Usuario() { }

    public Usuario(string nome, string email, string senhaHash, PerfilUsuario perfil)
    {
        Nome = Guard.Obrigatorio(nome, "O nome do usuário é obrigatório.");
        Email = Guard.Email(email, "E-mail do usuário inválido.");
        SenhaHash = Guard.Obrigatorio(senhaHash, "A senha do usuário é obrigatória.");
        Perfil = perfil;
        Ativo = true;
    }

    public void AlterarSenha(string novaSenhaHash)
    {
        SenhaHash = Guard.Obrigatorio(novaSenhaHash, "A senha do usuário é obrigatória.");
        MarcarComoAtualizado();
    }

    public void Ativar()
    {
        Ativo = true;
        MarcarComoAtualizado();
    }

    public void Desativar()
    {
        Ativo = false;
        MarcarComoAtualizado();
    }
}
