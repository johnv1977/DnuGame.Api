using DnuGame.Api.Common.Abstractions;
using DnuGame.Api.Common.DTO;

namespace DnuGame.Api.Modules.Ranking;

public class RankingService : IRankingService
{
    private readonly IPlayerStore _playerStore;

    public RankingService(IPlayerStore playerStore)
    {
        _playerStore = playerStore;
    }

    public async Task<IEnumerable<RankingEntry>> GetRankingAsync()
    {
        var players = await _playerStore.GetAllPlayersAsync();
        return players
            .OrderByDescending(p => p.Score)
            .ThenBy(p => p.Name)
            .Select(p => new RankingEntry(p.Id, p.Name, p.Score, p.IsOnline))
            .ToList();
    }

    public async Task NotifyRankingUpdatedAsync()
    {
        var ranking = await GetRankingAsync();
        // TODO: Broadcast to SignalR when Hub is implemented
        // await _hubContext.Clients.All.SendAsync("RankingUpdated", ranking);
    }
}
