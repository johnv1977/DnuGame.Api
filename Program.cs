using DnuGame.Api.Api.Extensions;
using DnuGame.Api.Modules.Auth;
using DnuGame.Api.Modules.Players;
using DnuGame.Api.Modules.GameRps;
using DnuGame.Api.Modules.Rooms;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Servicios base
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Configurar validación automática y JSON
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Health minimal
app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTimeOffset.UtcNow }))
   .WithName("Health")
   .WithOpenApi();

// Auth endpoints
app.MapAuthEndpoints();

// Players endpoints
app.MapPlayersEndpoints();

// Game RPS endpoints
app.MapGameRpsEndpoints();

// Rooms endpoints
app.MapRoomsEndpoints();

// SignalR Hub
app.MapHub<RpsHub>("/hubs/rps");

app.Run();
