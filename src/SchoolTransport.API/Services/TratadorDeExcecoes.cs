using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Exceptions;

namespace SchoolTransport.API.Services;

public class TratadorDeExcecoes(IProblemDetailsService problemDetails, ILogger<TratadorDeExcecoes> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, titulo) = exception switch
        {
            DomainException => (StatusCodes.Status400BadRequest, "Regra de negócio violada"),
            NaoEncontradoException => (StatusCodes.Status404NotFound, "Recurso não encontrado"),
            AcessoNegadoException => (StatusCodes.Status403Forbidden, "Acesso negado"),
            ConflitoException => (StatusCodes.Status409Conflict, "Conflito"),
            CredenciaisInvalidasException => (StatusCodes.Status401Unauthorized, "Não autorizado"),
            DbUpdateException => (StatusCodes.Status409Conflict, "Conflito ao salvar os dados"),
            _ => (StatusCodes.Status500InternalServerError, "Erro inesperado")
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Erro não tratado");

        httpContext.Response.StatusCode = status;

        return await problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = status,
                Title = titulo,
                Detail = status == StatusCodes.Status500InternalServerError || exception is DbUpdateException
                    ? "Não foi possível concluir a operação."
                    : exception.Message
            }
        });
    }
}
