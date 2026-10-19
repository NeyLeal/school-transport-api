using Microsoft.Extensions.Logging;
using SchoolTransport.Application.Common;
using SchoolTransport.Domain.Entities;
using SchoolTransport.Domain.Events;

namespace SchoolTransport.Application.Notificacoes;

public class NotificacaoViagemService(
    IAlunoRepository alunos,
    IMotoristaRepository motoristas,
    INotificador notificador,
    ILogger<NotificacaoViagemService> logger)
{
    public async Task ProcessarAsync(IDomainEvent evento, CancellationToken cancellationToken = default)
    {
        if (evento is not ViagemEventBase eventoViagem)
            return;

        var aluno = await alunos.ObterAsync(eventoViagem.AlunoId, cancellationToken);

        if (aluno is null)
        {
            logger.LogWarning("Aluno {AlunoId} não encontrado para o evento {Evento}", eventoViagem.AlunoId, evento.GetType().Name);
            return;
        }

        switch (evento)
        {
            case AusenciaInformadaEvent:
                await NotificarMotoristaAsync(aluno, "Ausência informada", $"{aluno.Nome} não irá na próxima viagem.", cancellationToken);
                break;
            case EnderecoViagemAlteradoEvent:
                await NotificarMotoristaAsync(aluno, "Endereço alterado", $"O endereço de {aluno.Nome} foi alterado para uma viagem.", cancellationToken);
                break;
            default:
                await NotificarResponsaveisAsync(aluno, MensagemParaResponsavel(evento, aluno), cancellationToken);
                break;
        }
    }

    private static string MensagemParaResponsavel(IDomainEvent evento, Aluno aluno) => evento switch
    {
        AlunoEmbarcouNaVanEvent => $"{aluno.Nome} embarcou na van.",
        AlunoChegouNaEscolaEvent => $"{aluno.Nome} chegou à escola {aluno.Escola?.Nome}.",
        AlunoSaiuDaEscolaEvent => $"{aluno.Nome} saiu da escola e está a caminho de casa.",
        AlunoChegouEmCasaEvent => $"{aluno.Nome} chegou em casa.",
        _ => $"Atualização da viagem de {aluno.Nome}."
    };

    private async Task NotificarResponsaveisAsync(Aluno aluno, string mensagem, CancellationToken cancellationToken)
    {
        foreach (var vinculo in aluno.Responsaveis.Where(r => r.RecebeNotificacoes && r.Responsavel is not null))
        {
            var responsavel = vinculo.Responsavel;
            await notificador.EnviarAsync(
                new Notificacao(responsavel.UsuarioId, responsavel.Nome, responsavel.Telefone, responsavel.Email, "SchoolTransport", mensagem),
                cancellationToken);
        }
    }

    private async Task NotificarMotoristaAsync(Aluno aluno, string titulo, string mensagem, CancellationToken cancellationToken)
    {
        var motorista = await motoristas.ObterPorIdAsync(aluno.MotoristaId, cancellationToken);

        if (motorista is null)
            return;

        await notificador.EnviarAsync(
            new Notificacao(motorista.UsuarioId, motorista.Nome, motorista.Telefone, string.Empty, titulo, mensagem),
            cancellationToken);
    }
}
