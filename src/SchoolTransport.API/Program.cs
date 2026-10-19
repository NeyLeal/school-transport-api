using SchoolTransport.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiComJwt();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApiComSwaggerUi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
