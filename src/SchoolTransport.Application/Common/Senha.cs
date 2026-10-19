using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.Application.Common;

internal static class Senha
{
    public const int TamanhoMinimo = 6;

    public static void Validar(string? senha)
    {
        if (string.IsNullOrWhiteSpace(senha) || senha.Length < TamanhoMinimo)
            throw new DomainException($"A senha deve ter pelo menos {TamanhoMinimo} caracteres.");
    }

    public static string NormalizarEmail(string? email) => (email ?? string.Empty).Trim().ToLowerInvariant();
}
