namespace SchoolTransport.API.Extensions;

public static class Perfis
{
    public const string Motorista = nameof(Motorista);
    public const string Ajudante = nameof(Ajudante);
    public const string Responsavel = nameof(Responsavel);
    public const string Equipe = Motorista + "," + Ajudante;
    public const string MotoristaOuResponsavel = Motorista + "," + Responsavel;
}
