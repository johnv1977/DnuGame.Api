using DnuGame.Api.Api.Extensions;
using DnuGame.Api.Modules.Auth;
using DnuGame.Api.Modules.Players;
using DnuGame.Api.Modules.GameRps;

var builder = WebApplication.CreateBuilder(args);

// Servicios base
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

// SignalR Hub
app.MapHub<RpsHub>("/hubs/rps");

app.Run();
