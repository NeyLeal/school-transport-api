namespace SchoolTransport.Infrastructure.Servicos;

public class FirebaseSettings
{
    public const string Secao = "Firebase";

    /// <summary>Caminho do JSON da conta de serviço. Vazio desativa o push e mantém só o log.</summary>
    public string CredenciaisArquivo { get; set; } = string.Empty;
}
