using System.Globalization;
using SchoolTransport.Contracts.Comum;

namespace SchoolTransport.UI.Services;

public static class Formatos
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");

    public static string Moeda(decimal valor) => valor.ToString("C", PtBr);

    public static string Data(DateOnly data) => data.ToString("dddd, d 'de' MMMM", PtBr);

    public static string Hora(DateTime? utc) => utc.HasValue ? utc.Value.ToLocalTime().ToString("HH:mm", PtBr) : string.Empty;

    public static string Parentesco(Parentesco parentesco) => parentesco switch
    {
        Contracts.Comum.Parentesco.Pai => "Pai",
        Contracts.Comum.Parentesco.Mae => "Mãe",
        Contracts.Comum.Parentesco.Avo => "Avó/Avô",
        _ => "Outro"
    };

    public static string Etapa(EtapaViagem etapa) => etapa switch
    {
        EtapaViagem.AguardandoIda => "Aguardando a van",
        EtapaViagem.EmTransitoParaEscola => "A caminho da escola",
        EtapaViagem.NaEscola => "Na escola",
        EtapaViagem.EmTransitoParaCasa => "A caminho de casa",
        EtapaViagem.Finalizada => "Em casa",
        _ => "Não vai hoje"
    };

    public static (AcaoViagem Acao, string Rotulo)? ProximaAcao(EtapaViagem etapa) => etapa switch
    {
        EtapaViagem.AguardandoIda => (AcaoViagem.Embarque, "Registrar embarque"),
        EtapaViagem.EmTransitoParaEscola => (AcaoViagem.ChegadaEscola, "Chegou na escola"),
        EtapaViagem.NaEscola => (AcaoViagem.SaidaEscola, "Saiu da escola"),
        EtapaViagem.EmTransitoParaCasa => (AcaoViagem.ChegadaCasa, "Chegou em casa"),
        _ => null
    };
}
