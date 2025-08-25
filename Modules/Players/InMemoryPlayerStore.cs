using System.Collections.Concurrent;
using DnuGame.Api.Common.Abstractions;
using DnuGame.Api.Common.DTO;
using DnuGame.Api.Modules.Players;

namespace DnuGame.Api.Modules.Players;

public class InMemoryPlayerStore : IPlayerStore
{
    private readonly ConcurrentDictionary<string, PlayerState> _players = new();

    public Task<IEnumerable<PlayerResponse>> GetAllPlayersAsync()
    {
        var players = _players.Values.ToList();
        var result = players.Select(p => new PlayerResponse(p.Id, p.Name, p.Score, p.IsOnline, p.LastSeen));
        return Task.FromResult<IEnumerable<PlayerResponse>>(result);
    }

    public Task<PlayerResponse?> GetPlayerAsync(string playerId)
    {
        if (_players.TryGetValue(playerId, out var player))
        {
            var result = new PlayerResponse(player.Id, player.Name, player.Score, player.IsOnline, player.LastSeen);
            return Task.FromResult<PlayerResponse?>(result);
        }
        return Task.FromResult<PlayerResponse?>(null);
    }

    public Task UpdatePlayerOnlineStatusAsync(string playerId, bool isOnline, string? connectionId = null)
    {
        _players.AddOrUpdate(playerId, 
            key => new PlayerState 
            { 
                Id = key, 
                Name = "Unknown", 
                IsOnline = isOnline, 
                LastSeen = DateTime.UtcNow,
                ConnectionId = connectionId 
            },
            (key, existing) => 
            {
                existing.IsOnline = isOnline;
                existing.LastSeen = DateTime.UtcNow;
                existing.ConnectionId = connectionId;
                return existing;
            });
        return Task.CompletedTask;
    }

    public Task UpdatePlayerScoreAsync(string playerId, int deltaScore)
    {
        _players.AddOrUpdate(playerId,
            key => new PlayerState 
            { 
                Id = key, 
                Name = "Unknown", 
                Score = Math.Max(0, deltaScore),
                LastSeen = DateTime.UtcNow 
            },
            (key, existing) => 
            {
                existing.Score = Math.Max(0, existing.Score + deltaScore);
                existing.LastSeen = DateTime.UtcNow;
                return existing;
            });
        return Task.CompletedTask;
    }

    public Task EnsurePlayerExistsAsync(string playerId, string name)
    {
        _players.AddOrUpdate(playerId,
            key => new PlayerState 
            { 
                Id = key, 
                Name = name, 
                Score = 0,
                IsOnline = false,
                LastSeen = DateTime.UtcNow 
            },
            (key, existing) => 
            {
                if (existing.Name == "Unknown" && !string.IsNullOrEmpty(name))
                {
                    existing.Name = name;
                }
                return existing;
            });
        return Task.CompletedTask;
    }
}
