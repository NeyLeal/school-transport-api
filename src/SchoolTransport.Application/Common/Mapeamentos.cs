using SchoolTransport.Contracts.Ajudantes;
using SchoolTransport.Contracts.Alunos;
using SchoolTransport.Contracts.Auth;
using SchoolTransport.Contracts.Escolas;
using SchoolTransport.Contracts.Responsaveis;
using SchoolTransport.Contracts.Viagens;
using SchoolTransport.Domain.Entities;
using Contrato = SchoolTransport.Contracts.Comum;

namespace SchoolTransport.Application.Common;

internal static class Mapeamentos
{
    public static UsuarioResponse ParaResposta(this Usuario usuario) =>
        new(usuario.Id, usuario.Nome, usuario.Email, (Contrato.Perfil)usuario.Perfil);

    public static EscolaResponse ParaResposta(this Escola escola) =>
        new(escola.Id, escola.Nome, escola.Endereco);

    public static ResponsavelResponse ParaResposta(this Responsavel responsavel) =>
        new(responsavel.Id, responsavel.Nome, responsavel.Telefone, responsavel.Email, responsavel.PossuiAcesso);

    public static AjudanteResponse ParaResposta(this Ajudante ajudante) =>
        new(ajudante.Id, ajudante.Nome, ajudante.Telefone);

    public static AlunoResponse ParaResposta(this Aluno aluno) =>
        new(
            aluno.Id,
            aluno.Nome,
            aluno.EscolaId,
            aluno.Escola?.Nome ?? string.Empty,
            aluno.DataNascimento,
            aluno.ValorMensalidade,
            aluno.EnderecoPadraoIda,
            aluno.EnderecoPadraoVolta,
            aluno.Ativo);

    public static VinculoResponsavelResponse ParaResposta(this AlunoResponsavel vinculo) =>
        new(
            vinculo.ResponsavelId,
            vinculo.Responsavel?.Nome ?? string.Empty,
            vinculo.Responsavel?.Telefone ?? string.Empty,
            (Contrato.Parentesco)vinculo.Parentesco,
            vinculo.PodeAlterarEndereco,
            vinculo.RecebeNotificacoes);

    public static ViagemResponse ParaResposta(this ViagemAluno viagem) =>
        new(
            viagem.Id,
            viagem.AlunoId,
            viagem.Aluno?.Nome ?? string.Empty,
            viagem.Aluno?.Escola?.Nome ?? string.Empty,
            viagem.Data,
            (Contrato.StatusPresenca)viagem.Status,
            (Contrato.EtapaViagem)viagem.Etapa,
            viagem.EnderecoIdaAtualizado ?? viagem.Aluno?.EnderecoPadraoIda ?? string.Empty,
            viagem.EnderecoVoltaAtualizado ?? viagem.Aluno?.EnderecoPadraoVolta ?? string.Empty,
            viagem.HorarioEntradaVanIda,
            viagem.HorarioChegadaEscola,
            viagem.HorarioEntradaVanVolta,
            viagem.HorarioChegadaEmCasa);
}
