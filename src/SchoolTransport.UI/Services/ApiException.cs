using System.Net;

namespace SchoolTransport.UI.Services;

public class ApiException(string mensagem, HttpStatusCode status) : Exception(mensagem)
{
    public HttpStatusCode Status { get; } = status;
}
