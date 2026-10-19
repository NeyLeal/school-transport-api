# SchoolTransport

Gestão de transporte escolar: o motorista registra cada etapa da viagem (embarque, chegada na escola, saída, chegada em casa) e os responsáveis acompanham e recebem avisos.

## Estrutura

| Projeto | Papel |
| --- | --- |
| `SchoolTransport.Domain` | Entidades, regras e eventos de domínio |
| `SchoolTransport.Application` | Casos de uso e abstrações (repositórios, token, relógio, notificador) |
| `SchoolTransport.Contracts` | DTOs compartilhados entre API, web e app |
| `SchoolTransport.Infrastructure` | EF Core (SQL Server), JWT, BCrypt e outbox |
| `SchoolTransport.API` | API REST com autenticação JWT por perfil |
| `SchoolTransport.Worker` | Processa o outbox e dispara as notificações |
| `SchoolTransport.UI` | Telas Blazor compartilhadas |
| `SchoolTransport.Web` | Host Blazor Server para PC |
| `SchoolTransport.App` | App MAUI Blazor Hybrid (Android e Windows) |

## Requisitos

- .NET SDK 10
- SQL Server local
- Workload MAUI para o app: `dotnet workload install maui`

## Como rodar

```bash
dotnet tool restore
dotnet run --project src/SchoolTransport.API --launch-profile https
dotnet run --project src/SchoolTransport.Worker
dotnet run --project src/SchoolTransport.Web --launch-profile https
```

Em desenvolvimento a API aplica as migrations ao iniciar. Para criar uma nova: `dotnet ef migrations add <Nome> -p src/SchoolTransport.Infrastructure -s src/SchoolTransport.API -o Persistence/Migrations`. A documentação fica em `https://localhost:7260/swagger`.

O app usa `http://10.0.2.2:5017` no emulador Android e `https://localhost:7260` no Windows (`Configuracao.cs`). Em celular físico, troque pelo IP da máquina na rede.

## Próximos passos

- Push e WhatsApp no lugar do `LogNotificador`
- Atualização em tempo real com SignalR
- Acesso para ajudantes e mensalidades
