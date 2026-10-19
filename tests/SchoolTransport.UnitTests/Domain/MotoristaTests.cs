using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.UnitTests.Domain;

public class MotoristaTests
{
    [Fact]
    public void Deve_adicionar_e_remover_ajudante()
    {
        var motorista = new Motorista(Guid.NewGuid(), "Carlos", "12345678900", "11999999999");

        var ajudante = motorista.AdicionarAjudante("Bia", "11988887777");
        motorista.RemoverAjudante(ajudante.Id);

        Assert.Empty(motorista.Ajudantes);
    }

    [Fact]
    public void Nao_deve_remover_ajudante_inexistente()
    {
        var motorista = new Motorista(Guid.NewGuid(), "Carlos", "12345678900", "11999999999");

        Assert.Throws<DomainException>(() => motorista.RemoverAjudante(Guid.NewGuid()));
    }

    [Fact]
    public void Responsavel_com_acesso_nao_troca_email()
    {
        var responsavel = new Responsavel(Guid.NewGuid(), "Marta", "11977776666", "marta@email.com");
        responsavel.VincularUsuario(Guid.NewGuid());

        Assert.Throws<DomainException>(() => responsavel.Atualizar("Marta", "11977776666", "outro@email.com"));
    }
}
