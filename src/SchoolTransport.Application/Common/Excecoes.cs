namespace SchoolTransport.Application.Common;

public class NaoEncontradoException(string mensagem) : Exception(mensagem);

public class AcessoNegadoException(string mensagem) : Exception(mensagem);

public class ConflitoException(string mensagem) : Exception(mensagem);

public class CredenciaisInvalidasException() : Exception("E-mail ou senha inválidos.");
