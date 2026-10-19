using SchoolTransport.Domain.Common;
using SchoolTransport.Domain.Enums;
using SchoolTransport.Domain.Events;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.Domain.Entities;

public class ViagemAluno : AggregateRoot
{
    public const int HorasMinimasAlteracaoEndereco = 24;

    public Guid AlunoId { get; private set; }
    public Aluno Aluno { get; private set; } = null!;
    public Guid MotoristaId { get; private set; }
    public DateOnly Data { get; private set; }
    public StatusPresenca Status { get; private set; } = StatusPresenca.Confirmado;
    public EtapaViagem Etapa { get; private set; } = EtapaViagem.AguardandoIda;

    public DateTime? HorarioEntradaVanIda { get; private set; }
    public DateTime? HorarioChegadaEscola { get; private set; }
    public DateTime? HorarioEntradaVanVolta { get; private set; }
    public DateTime? HorarioChegadaEmCasa { get; private set; }

    public string? EnderecoIdaAtualizado { get; private set; }
    public string? EnderecoVoltaAtualizado { get; private set; }

    private ViagemAluno() { }

    public ViagemAluno(Guid alunoId, Guid motoristaId, DateOnly data)
    {
        AlunoId = Guard.Obrigatorio(alunoId, "A viagem precisa estar vinculada a um aluno.");
        MotoristaId = Guard.Obrigatorio(motoristaId, "A viagem precisa estar vinculada a um motorista.");
        Data = data;
    }

    public void RegistrarEmbarque()
    {
        AvancarEtapa(EtapaViagem.AguardandoIda, EtapaViagem.EmTransitoParaEscola, "A criança não está aguardando a ida.");
        HorarioEntradaVanIda = DateTime.UtcNow;
        AdicionarEvento(new AlunoEmbarcouNaVanEvent(Id, AlunoId, HorarioEntradaVanIda.Value));
    }

    public void RegistrarChegadaNaEscola()
    {
        AvancarEtapa(EtapaViagem.EmTransitoParaEscola, EtapaViagem.NaEscola, "A criança não estava em trânsito para a escola.");
        HorarioChegadaEscola = DateTime.UtcNow;
        AdicionarEvento(new AlunoChegouNaEscolaEvent(Id, AlunoId, HorarioChegadaEscola.Value));
    }

    public void RegistrarSaidaDaEscola()
    {
        AvancarEtapa(EtapaViagem.NaEscola, EtapaViagem.EmTransitoParaCasa, "A criança não estava na escola.");
        HorarioEntradaVanVolta = DateTime.UtcNow;
        AdicionarEvento(new AlunoSaiuDaEscolaEvent(Id, AlunoId, HorarioEntradaVanVolta.Value));
    }

    public void RegistrarChegadaEmCasa()
    {
        AvancarEtapa(EtapaViagem.EmTransitoParaCasa, EtapaViagem.Finalizada, "A criança não estava em retorno para casa.");
        HorarioChegadaEmCasa = DateTime.UtcNow;
        AdicionarEvento(new AlunoChegouEmCasaEvent(Id, AlunoId, HorarioChegadaEmCasa.Value));
    }

    public void InformarAusencia()
    {
        if (Etapa != EtapaViagem.AguardandoIda)
            throw new DomainException("Só é possível informar ausência antes do embarque.");

        Status = StatusPresenca.Ausente;
        Etapa = EtapaViagem.Cancelada;
        MarcarComoAtualizado();
        AdicionarEvento(new AusenciaInformadaEvent(Id, AlunoId, DateTime.UtcNow));
    }

    public void ConfirmarPresenca()
    {
        if (Status != StatusPresenca.Ausente || Etapa != EtapaViagem.Cancelada)
            throw new DomainException("A viagem não está marcada como ausência.");

        Status = StatusPresenca.Confirmado;
        Etapa = EtapaViagem.AguardandoIda;
        MarcarComoAtualizado();
    }

    public void AlterarEndereco(Trajeto trajeto, string novoEndereco, DateTime agora)
    {
        var endereco = Guard.Obrigatorio(novoEndereco, "O novo endereço é obrigatório.");
        var inicioDaViagem = Data.ToDateTime(TimeOnly.MinValue);

        if ((inicioDaViagem - agora).TotalHours < HorasMinimasAlteracaoEndereco)
            throw new DomainException($"Alterações de endereço só podem ser feitas com {HorasMinimasAlteracaoEndereco}h de antecedência.");

        if (trajeto == Trajeto.Ida)
            EnderecoIdaAtualizado = endereco;
        else
            EnderecoVoltaAtualizado = endereco;

        MarcarComoAtualizado();
        AdicionarEvento(new EnderecoViagemAlteradoEvent(Id, AlunoId, DateTime.UtcNow));
    }

    private void AvancarEtapa(EtapaViagem esperada, EtapaViagem proxima, string mensagem)
    {
        if (Status != StatusPresenca.Confirmado)
            throw new DomainException("A presença do aluno não está confirmada para esta viagem.");

        if (Etapa != esperada)
            throw new DomainException(mensagem);

        Etapa = proxima;
        MarcarComoAtualizado();
    }
}
