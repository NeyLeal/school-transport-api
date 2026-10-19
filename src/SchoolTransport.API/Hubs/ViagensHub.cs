using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SchoolTransport.API.Extensions;
using SchoolTransport.Application.Common;
using SchoolTransport.Infrastructure.Security;

namespace SchoolTransport.API.Hubs;

[Authorize]
public class ViagensHub(IAlunoRepository alunos) : Hub
{
    public static string GrupoMotorista(Guid motoristaId) => $"motorista:{motoristaId}";
    public static string GrupoAluno(Guid alunoId) => $"aluno:{alunoId}";

    public override async Task OnConnectedAsync()
    {
        var usuario = Context.User!;

        // Grupos definidos na conexão: um aluno vinculado depois só aparece após reconectar.
        if (usuario.IsInRole(Perfis.Responsavel))
        {
            if (Guid.TryParse(usuario.FindFirstValue(ClaimsPersonalizadas.ResponsavelId), out var responsavelId))
            {
                foreach (var aluno in await alunos.ListarPorResponsavelAsync(responsavelId, Context.ConnectionAborted))
                    await Groups.AddToGroupAsync(Context.ConnectionId, GrupoAluno(aluno.Id), Context.ConnectionAborted);
            }
        }
        else if (Guid.TryParse(usuario.FindFirstValue(ClaimsPersonalizadas.MotoristaId), out var motoristaId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GrupoMotorista(motoristaId), Context.ConnectionAborted);
        }

        await base.OnConnectedAsync();
    }
}
