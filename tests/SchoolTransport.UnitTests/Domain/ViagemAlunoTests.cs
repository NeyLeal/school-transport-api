using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Enums;
using SchoolTransport.Domain.Events;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.UnitTests.Domain;

public class ViagemAlunoTests
{
    private static readonly DateOnly Hoje = new(2026, 10, 19);

    private static ViagemAluno NovaViagem(DateOnly? data = null) =>
        new(Guid.NewGuid(), Guid.NewGuid(), data ?? Hoje);

    [Fact]
    public void Deve_percorrer_todas_as_etapas_e_gerar_eventos()
    {
        var viagem = NovaViagem();

        viagem.RegistrarEmbarque();
        viagem.RegistrarChegadaNaEscola();
        viagem.RegistrarSaidaDaEscola();
        viagem.RegistrarChegadaEmCasa();

        Assert.Equal(EtapaViagem.Finalizada, viagem.Etapa);
        Assert.NotNull(viagem.HorarioChegadaEmCasa);
        Assert.Collection(viagem.Eventos,
            e => Assert.IsType<AlunoEmbarcouNaVanEvent>(e),
            e => Assert.IsType<AlunoChegouNaEscolaEvent>(e),
            e => Assert.IsType<AlunoSaiuDaEscolaEvent>(e),
            e => Assert.IsType<AlunoChegouEmCasaEvent>(e));
    }

    [Fact]
    public void Nao_deve_registrar_chegada_na_escola_sem_embarque()
    {
        var viagem = NovaViagem();

        Assert.Throws<DomainException>(viagem.RegistrarChegadaNaEscola);
    }

    [Fact]
    public void Ausencia_deve_cancelar_viagem_e_bloquear_embarque()
    {
        var viagem = NovaViagem();

        viagem.InformarAusencia();

        Assert.Equal(StatusPresenca.Ausente, viagem.Status);
        Assert.Equal(EtapaViagem.Cancelada, viagem.Etapa);
        Assert.Throws<DomainException>(viagem.RegistrarEmbarque);
    }

    [Fact]
    public void Confirmar_presenca_deve_reabrir_viagem_ausente()
    {
        var viagem = NovaViagem();
        viagem.InformarAusencia();

        viagem.ConfirmarPresenca();

        Assert.Equal(StatusPresenca.Confirmado, viagem.Status);
        Assert.Equal(EtapaViagem.AguardandoIda, viagem.Etapa);
    }

    [Fact]
    public void Nao_deve_informar_ausencia_apos_embarque()
    {
        var viagem = NovaViagem();
        viagem.RegistrarEmbarque();

        Assert.Throws<DomainException>(viagem.InformarAusencia);
    }

    [Fact]
    public void Deve_alterar_endereco_com_24h_de_antecedencia()
    {
        var viagem = NovaViagem(Hoje.AddDays(2));
        var agora = Hoje.ToDateTime(new TimeOnly(10, 0));

        viagem.AlterarEndereco(Trajeto.Volta, "Rua Nova, 10", agora);

        Assert.Equal("Rua Nova, 10", viagem.EnderecoVoltaAtualizado);
        Assert.Null(viagem.EnderecoIdaAtualizado);
    }

    [Fact]
    public void Nao_deve_alterar_endereco_com_menos_de_24h()
    {
        var viagem = NovaViagem(Hoje.AddDays(1));
        var agora = Hoje.ToDateTime(new TimeOnly(10, 0));

        Assert.Throws<DomainException>(() => viagem.AlterarEndereco(Trajeto.Ida, "Rua Nova, 10", agora));
    }
}
