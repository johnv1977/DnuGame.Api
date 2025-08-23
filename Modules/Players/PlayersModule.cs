using DnuGame.Api.Common.Abstractions;
using DnuGame.Api.Common.DTO;
using Microsoft.AspNetCore.Authorization;

namespace DnuGame.Api.Modules.Players;

public static class PlayersModule
{
    public static IEndpointRouteBuilder MapPlayersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/players").WithTags("Players").RequireAuthorization();

        group.MapGet("/", GetPlayersAsync)
            .WithName("GetPlayers")
            .WithOpenApi();

        group.MapGet("/{id}", GetPlayerAsync)
            .WithName("GetPlayer")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> GetPlayersAsync(IPlayerStore playerStore)
    {
        var players = await playerStore.GetAllPlayersAsync();
        return Results.Ok(players);
    }

    private static async Task<IResult> GetPlayerAsync(string id, IPlayerStore playerStore)
    {
        var player = await playerStore.GetPlayerAsync(id);
        return player != null ? Results.Ok(player) : Results.NotFound();
    }
}
