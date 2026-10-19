using System.Text.Json.Serialization;

namespace SchoolTransport.Contracts.Comum;

[JsonConverter(typeof(JsonStringEnumConverter<Perfil>))]
public enum Perfil
{
    Admin = 1,
    Motorista = 2,
    Responsavel = 3,
    Ajudante = 4
}

[JsonConverter(typeof(JsonStringEnumConverter<EtapaViagem>))]
public enum EtapaViagem
{
    AguardandoIda = 1,
    EmTransitoParaEscola = 2,
    NaEscola = 3,
    EmTransitoParaCasa = 4,
    Finalizada = 5,
    Cancelada = 6
}

[JsonConverter(typeof(JsonStringEnumConverter<StatusPresenca>))]
public enum StatusPresenca
{
    Confirmado = 1,
    Ausente = 2,
    Ferias = 3
}

[JsonConverter(typeof(JsonStringEnumConverter<Parentesco>))]
public enum Parentesco
{
    Pai = 1,
    Mae = 2,
    Avo = 3,
    Outro = 4
}

[JsonConverter(typeof(JsonStringEnumConverter<Trajeto>))]
public enum Trajeto
{
    Ida = 1,
    Volta = 2
}

[JsonConverter(typeof(JsonStringEnumConverter<AcaoViagem>))]
public enum AcaoViagem
{
    Embarque = 1,
    ChegadaEscola = 2,
    SaidaEscola = 3,
    ChegadaCasa = 4
}
