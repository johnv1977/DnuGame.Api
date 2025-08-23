using DnuGame.Api.Common.Abstractions;

namespace DnuGame.Api.Common.Abstractions;

public interface IGameRpsService
{
    Task<bool> EnqueuePlayerAsync(string playerId, string move);
    Task ProcessMatchesAsync();
}
