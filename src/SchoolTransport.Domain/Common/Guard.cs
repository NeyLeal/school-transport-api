using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.Domain.Common;

internal static class Guard
{
    public static string Obrigatorio(string? valor, string mensagem)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException(mensagem);

        return valor.Trim();
    }

    public static Guid Obrigatorio(Guid valor, string mensagem)
    {
        if (valor == Guid.Empty)
            throw new DomainException(mensagem);

        return valor;
    }

    public static string Email(string? valor, string mensagem)
    {
        var email = Obrigatorio(valor, mensagem).ToLowerInvariant();

        if (!email.Contains('@') || email.StartsWith('@') || email.EndsWith('@'))
            throw new DomainException(mensagem);

        return email;
    }
}
