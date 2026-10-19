namespace SchoolTransport.Infrastructure.Outbox;

public class OutboxMessage
{
    public const int MaximoTentativas = 5;

    public Guid Id { get; private set; }
    public string Tipo { get; private set; } = string.Empty;
    public string Conteudo { get; private set; } = string.Empty;
    public DateTime OcorridoEm { get; private set; }
    public DateTime? ProcessadoEm { get; private set; }
    public int Tentativas { get; private set; }
    public string? Erro { get; private set; }

    private OutboxMessage() { }

    public OutboxMessage(string tipo, string conteudo, DateTime ocorridoEm)
    {
        Id = Guid.NewGuid();
        Tipo = tipo;
        Conteudo = conteudo;
        OcorridoEm = ocorridoEm;
    }

    public void MarcarProcessada()
    {
        ProcessadoEm = DateTime.UtcNow;
        Erro = null;
    }

    public void RegistrarFalha(string erro)
    {
        Tentativas++;
        Erro = erro.Length > 2000 ? erro[..2000] : erro;

        if (Tentativas >= MaximoTentativas)
            ProcessadoEm = DateTime.UtcNow;
    }
}
