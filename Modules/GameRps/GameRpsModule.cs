using DnuGame.Api.Common.DTO;
using Microsoft.AspNetCore.Authorization;

namespace DnuGame.Api.Modules.GameRps;

public static class GameRpsModule
{
    public static IEndpointRouteBuilder MapGameRpsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/game/rps").WithTags("Game RPS");

        group.MapGet("/hub/info", GetHubInfo)
            .WithName("GetRpsHubInfo")
            .WithOpenApi()
            .WithSummary("Get Rock-Paper-Scissors SignalR Hub Information")
            .WithDescription("Returns detailed information about the RPS SignalR Hub, including available methods, events, and connection details.")
            .Produces<RpsHubInfoResponse>(StatusCodes.Status200OK);

        return app;
    }

    private static IResult GetHubInfo()
    {
        var hubInfo = new RpsHubInfoResponse
        {
            HubUrl = "/hubs/rps",
            RequiresAuthentication = true,
            ClientMethods = new[]
            {
                new HubMethodInfo("Join", "Connect to the game hub", Array.Empty<HubParameter>()),
                new HubMethodInfo("Play", "Make a move in the game", new[]
                {
                    new HubParameter("move", "string", "The move to play: 'rock', 'paper', or 'scissors'", true)
                }),
                new HubMethodInfo("GetRanking", "Get current player ranking", Array.Empty<HubParameter>())
            },
            ServerEvents = new[]
            {
                new HubEventInfo("PlayerJoined", "Fired when a player joins the game", "Player object"),
                new HubEventInfo("PlayerLeft", "Fired when a player leaves the game", "Player ID"),
                new HubEventInfo("MoveRegistered", "Fired when player's move is registered", "Move confirmation object"),
                new HubEventInfo("RankingUpdated", "Fired when ranking is updated", "Array of ranking entries"),
                new HubEventInfo("GameResult", "Fired when a game round completes", "Game result object")
            }
        };

        return Results.Ok(hubInfo);
    }
}

public record RpsHubInfoResponse
{
    public string HubUrl { get; init; } = string.Empty;
    public bool RequiresAuthentication { get; init; }
    public HubMethodInfo[] ClientMethods { get; init; } = Array.Empty<HubMethodInfo>();
    public HubEventInfo[] ServerEvents { get; init; } = Array.Empty<HubEventInfo>();
}

public record HubMethodInfo(
    string Name,
    string Description,
    HubParameter[] Parameters
);

public record HubParameter(
    string Name,
    string Type,
    string Description,
    bool Required
);

public record HubEventInfo(
    string Name,
    string Description,
    string PayloadType
);
