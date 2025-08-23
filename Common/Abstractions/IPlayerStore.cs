using DnuGame.Api.Common.DTO;

namespace DnuGame.Api.Common.Abstractions;

public interface IPlayerStore
{
    Task<IEnumerable<PlayerResponse>> GetAllPlayersAsync();
    Task<PlayerResponse?> GetPlayerAsync(string playerId);
    Task UpdatePlayerOnlineStatusAsync(string playerId, bool isOnline, string? connectionId = null);
    Task UpdatePlayerScoreAsync(string playerId, int deltaScore);
    Task EnsurePlayerExistsAsync(string playerId, string name);
}

public interface IRankingService
{
    Task<IEnumerable<RankingEntry>> GetRankingAsync();
    Task NotifyRankingUpdatedAsync();
}
