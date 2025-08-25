using DnuGame.Api.Common.Abstractions;
using DnuGame.Api.Modules.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace DnuGame.Api.Modules.GameRps;

/// <summary>
/// SignalR Hub for Rock-Paper-Scissors game functionality.
/// Requires authentication for all methods.
/// Hub URL: /hubs/rps
/// </summary>
[Authorize]
public class RpsHub : Hub
{
    private readonly IPlayerStore _playerStore;
    private readonly IRankingService _rankingService;
    private readonly IGameRpsService _gameService;
    private readonly UserManager<ApplicationUser> _userManager;

    public RpsHub(
        IPlayerStore playerStore, 
        IRankingService rankingService,
        IGameRpsService gameService,
        UserManager<ApplicationUser> userManager)
    {
        _playerStore = playerStore;
        _rankingService = rankingService;
        _gameService = gameService;
        _userManager = userManager;
    }

    /// <summary>
    /// Join the game hub. This method should be called first when connecting.
    /// Registers the player and notifies all connected clients.
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    /// <exception cref="HubException">Thrown when user is not authenticated or not found</exception>
    public async Task Join()
    {
        var userId = Context.UserIdentifier;
        if (string.IsNullOrEmpty(userId))
        {
            throw new HubException("User not authenticated");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new HubException("User not found");
        }

        var displayName = user.DisplayName ?? user.UserName ?? "Unknown";
        
        await _playerStore.EnsurePlayerExistsAsync(userId, displayName);
        await _playerStore.UpdatePlayerOnlineStatusAsync(userId, true, Context.ConnectionId);

        var player = await _playerStore.GetPlayerAsync(userId);
        
        // Notify all clients
        await Clients.All.SendAsync("PlayerJoined", player);
        
        // Send current ranking to the joining player
        var ranking = await _rankingService.GetRankingAsync();
        await Clients.Caller.SendAsync("RankingUpdated", ranking);
    }

    /// <summary>
    /// Submit a move for the Rock-Paper-Scissors game.
    /// </summary>
    /// <param name="move">The move to play. Valid values: "rock", "paper", "scissors" (case insensitive)</param>
    /// <returns>Task representing the async operation</returns>
    /// <exception cref="HubException">Thrown when user is not authenticated, move is invalid, or registration fails</exception>
    public async Task Play(string move)
    {
        var userId = Context.UserIdentifier;
        if (string.IsNullOrEmpty(userId))
        {
            throw new HubException("User not authenticated");
        }

        if (!IsValidMove(move))
        {
            throw new HubException("Invalid move. Use 'rock', 'paper', or 'scissors'");
        }

        var success = await _gameService.EnqueuePlayerAsync(userId, move);
        if (!success)
        {
            throw new HubException("Failed to register move");
        }

        await Clients.Caller.SendAsync("MoveRegistered", new { move, timestamp = DateTime.UtcNow });
    }

    /// <summary>
    /// Get the current player ranking.
    /// Sends the ranking data to the calling client.
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    public async Task GetRanking()
    {
        var ranking = await _rankingService.GetRankingAsync();
        await Clients.Caller.SendAsync("RankingUpdated", ranking);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await _playerStore.UpdatePlayerOnlineStatusAsync(userId, false);
            
            var player = await _playerStore.GetPlayerAsync(userId);
            if (player != null)
            {
                await Clients.All.SendAsync("PlayerLeft", player.Id);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    private static bool IsValidMove(string move)
    {
        return move?.ToLowerInvariant() is "rock" or "paper" or "scissors";
    }
}
