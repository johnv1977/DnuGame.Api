using DnuGame.Api.Common.Abstractions;
using DnuGame.Api.Common.DTO;
using DnuGame.Api.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace DnuGame.Api.Modules.GameRps;

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
