using SchoolTransport.UI.Services;
using SchoolTransport.Web.Components;
using SchoolTransport.Web.Services;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration["Api:BaseUrl"]
    ?? throw new InvalidOperationException("Api:BaseUrl não configurada.");

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<ISessaoStorage, LocalStorageSessao>();
builder.Services.AddSchoolTransportUi(new Uri(apiBaseUrl));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/nao-encontrada", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/nao-encontrada");
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(SchoolTransport.UI.Routes).Assembly);

app.Run();
