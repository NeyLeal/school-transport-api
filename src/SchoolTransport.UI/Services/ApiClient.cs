using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SchoolTransport.Contracts.Ajudantes;
using SchoolTransport.Contracts.Alunos;
using SchoolTransport.Contracts.Auth;
using SchoolTransport.Contracts.Comum;
using SchoolTransport.Contracts.Dispositivos;
using SchoolTransport.Contracts.Escolas;
using SchoolTransport.Contracts.Responsaveis;
using SchoolTransport.Contracts.Viagens;

namespace SchoolTransport.UI.Services;

public class ApiClient(HttpClient http, SessaoService sessao)
{
    public Task<LoginResponse> LoginAsync(LoginRequest request) =>
        EnviarAsync<LoginResponse>(HttpMethod.Post, "api/auth/login", request);

    public Task<LoginResponse> RegistrarMotoristaAsync(RegistrarMotoristaRequest request) =>
        EnviarAsync<LoginResponse>(HttpMethod.Post, "api/auth/registrar-motorista", request);

    public Task<List<EscolaResponse>> ListarEscolasAsync() =>
        EnviarAsync<List<EscolaResponse>>(HttpMethod.Get, "api/escolas");

    public Task<EscolaResponse> CriarEscolaAsync(EscolaRequest request) =>
        EnviarAsync<EscolaResponse>(HttpMethod.Post, "api/escolas", request);

    public Task<EscolaResponse> AtualizarEscolaAsync(Guid id, EscolaRequest request) =>
        EnviarAsync<EscolaResponse>(HttpMethod.Put, $"api/escolas/{id}", request);

    public Task<List<AlunoResponse>> ListarAlunosAsync(bool incluirInativos) =>
        EnviarAsync<List<AlunoResponse>>(HttpMethod.Get, $"api/alunos?incluirInativos={incluirInativos}");

    public Task<AlunoDetalheResponse> ObterAlunoAsync(Guid id) =>
        EnviarAsync<AlunoDetalheResponse>(HttpMethod.Get, $"api/alunos/{id}");

    public Task<AlunoResponse> CriarAlunoAsync(AlunoRequest request) =>
        EnviarAsync<AlunoResponse>(HttpMethod.Post, "api/alunos", request);

    public Task<AlunoResponse> AtualizarAlunoAsync(Guid id, AlunoRequest request) =>
        EnviarAsync<AlunoResponse>(HttpMethod.Put, $"api/alunos/{id}", request);

    public Task DefinirAlunoAtivoAsync(Guid id, bool ativo) =>
        EnviarAsync(HttpMethod.Post, $"api/alunos/{id}/{(ativo ? "reativar" : "desativar")}");

    public Task<AlunoDetalheResponse> VincularResponsavelAsync(Guid alunoId, VinculoResponsavelRequest request) =>
        EnviarAsync<AlunoDetalheResponse>(HttpMethod.Post, $"api/alunos/{alunoId}/responsaveis", request);

    public Task DesvincularResponsavelAsync(Guid alunoId, Guid responsavelId) =>
        EnviarAsync(HttpMethod.Delete, $"api/alunos/{alunoId}/responsaveis/{responsavelId}");

    public Task<List<ResponsavelResponse>> ListarResponsaveisAsync() =>
        EnviarAsync<List<ResponsavelResponse>>(HttpMethod.Get, "api/responsaveis");

    public Task<ResponsavelResponse> CriarResponsavelAsync(ResponsavelRequest request) =>
        EnviarAsync<ResponsavelResponse>(HttpMethod.Post, "api/responsaveis", request);

    public Task<ResponsavelResponse> AtualizarResponsavelAsync(Guid id, ResponsavelRequest request) =>
        EnviarAsync<ResponsavelResponse>(HttpMethod.Put, $"api/responsaveis/{id}", request);

    public Task<ResponsavelResponse> CriarAcessoResponsavelAsync(Guid id, CriarAcessoRequest request) =>
        EnviarAsync<ResponsavelResponse>(HttpMethod.Post, $"api/responsaveis/{id}/acesso", request);

    public Task<List<AjudanteResponse>> ListarAjudantesAsync() =>
        EnviarAsync<List<AjudanteResponse>>(HttpMethod.Get, "api/ajudantes");

    public Task<AjudanteResponse> AdicionarAjudanteAsync(AjudanteRequest request) =>
        EnviarAsync<AjudanteResponse>(HttpMethod.Post, "api/ajudantes", request);

    public Task RemoverAjudanteAsync(Guid id) =>
        EnviarAsync(HttpMethod.Delete, $"api/ajudantes/{id}");

    public Task<List<ViagemResponse>> ListarViagensAsync(DateOnly data) =>
        EnviarAsync<List<ViagemResponse>>(HttpMethod.Get, $"api/viagens?data={data:yyyy-MM-dd}");

    public Task<GerarViagensResponse> GerarViagensAsync(DateOnly data) =>
        EnviarAsync<GerarViagensResponse>(HttpMethod.Post, "api/viagens/gerar", new GerarViagensRequest { Data = data });

    public Task<ViagemResponse> RegistrarAcaoAsync(Guid viagemId, AcaoViagem acao) =>
        EnviarAsync<ViagemResponse>(HttpMethod.Post, $"api/viagens/{viagemId}/acoes/{acao}");

    public Task<ViagemResponse> InformarAusenciaAsync(PresencaRequest request) =>
        EnviarAsync<ViagemResponse>(HttpMethod.Post, "api/viagens/ausencia", request);

    public Task<ViagemResponse> ConfirmarPresencaAsync(PresencaRequest request) =>
        EnviarAsync<ViagemResponse>(HttpMethod.Post, "api/viagens/presenca", request);

    public Task<ViagemResponse> AlterarEnderecoAsync(AlterarEnderecoRequest request) =>
        EnviarAsync<ViagemResponse>(HttpMethod.Put, "api/viagens/endereco", request);

    public Task<List<MeuAlunoResponse>> ListarMeusAlunosAsync() =>
        EnviarAsync<List<MeuAlunoResponse>>(HttpMethod.Get, "api/viagens/meus-alunos");

    public Task RegistrarDispositivoAsync(RegistrarDispositivoRequest request) =>
        EnviarAsync(HttpMethod.Post, "api/dispositivos", request);

    public Task RemoverDispositivoAsync(RemoverDispositivoRequest request) =>
        EnviarAsync(HttpMethod.Post, "api/dispositivos/remover", request);

    private async Task<T> EnviarAsync<T>(HttpMethod metodo, string url, object? corpo = null)
    {
        using var resposta = await ExecutarAsync(metodo, url, corpo);
        return (await resposta.Content.ReadFromJsonAsync<T>())!;
    }

    private async Task EnviarAsync(HttpMethod metodo, string url, object? corpo = null)
    {
        using var resposta = await ExecutarAsync(metodo, url, corpo);
    }

    private async Task<HttpResponseMessage> ExecutarAsync(HttpMethod metodo, string url, object? corpo)
    {
        var requisicao = new HttpRequestMessage(metodo, url);

        if (corpo is not null)
            requisicao.Content = JsonContent.Create(corpo, corpo.GetType());

        if (sessao.Token is { } token)
            requisicao.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage resposta;

        try
        {
            resposta = await http.SendAsync(requisicao);
        }
        catch (HttpRequestException)
        {
            throw new ApiException("Não foi possível conectar ao servidor. Verifique sua conexão e tente novamente.", HttpStatusCode.ServiceUnavailable);
        }

        if (resposta.IsSuccessStatusCode)
            return resposta;

        var mensagem = await LerMensagemDeErroAsync(resposta);
        resposta.Dispose();

        if (resposta.StatusCode == HttpStatusCode.Unauthorized && sessao.Autenticado)
            await sessao.SairAsync();

        throw new ApiException(mensagem, resposta.StatusCode);
    }

    private static async Task<string> LerMensagemDeErroAsync(HttpResponseMessage resposta)
    {
        try
        {
            using var documento = JsonDocument.Parse(await resposta.Content.ReadAsStringAsync());

            if (documento.RootElement.TryGetProperty("detail", out var detalhe) && detalhe.GetString() is { Length: > 0 } texto)
                return texto;

            if (documento.RootElement.TryGetProperty("errors", out var erros))
            {
                var primeiro = erros.EnumerateObject().SelectMany(e => e.Value.EnumerateArray()).FirstOrDefault();
                if (primeiro.ValueKind == JsonValueKind.String)
                    return primeiro.GetString()!;
            }

            if (documento.RootElement.TryGetProperty("title", out var titulo) && titulo.GetString() is { Length: > 0 } t)
                return t;
        }
        catch (JsonException)
        {
        }

        return resposta.StatusCode switch
        {
            HttpStatusCode.Unauthorized => "Sua sessão expirou. Entre novamente.",
            HttpStatusCode.Forbidden => "Você não tem permissão para esta ação.",
            _ => "Não foi possível concluir a operação."
        };
    }
}
