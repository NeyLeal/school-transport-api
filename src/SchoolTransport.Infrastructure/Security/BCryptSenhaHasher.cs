using SchoolTransport.Application.Common;

namespace SchoolTransport.Infrastructure.Security;

public class BCryptSenhaHasher : ISenhaHasher
{
    public string Gerar(string senha) => BCrypt.Net.BCrypt.HashPassword(senha);

    public bool Verificar(string senha, string hash) => BCrypt.Net.BCrypt.Verify(senha, hash);
}
