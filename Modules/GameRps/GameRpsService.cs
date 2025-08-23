using System.Collections.Concurrent;
using DnuGame.Api.Common.Abstractions;
using DnuGame.Api.Common.DTO;
using DnuGame.Api.Modules.Players;
using Microsoft.AspNetCore.SignalR;

namespace DnuGame.Api.Modules.GameRps;

public class GameRpsService : IGameRpsService
{
    private readonly ConcurrentQueue<PendingMatch> _waitingPlayers = new();
    private readonly IPlayerStore _playerStore;
    private readonly IRankingService _rankingService;
    private readonly IHubContext<RpsHub> _hubContext;

    // Configuración de puntajes
    private const int WinScore = 3;
    private const int DrawScore = 1;
    private const int LoseScore = 0;

    public GameRpsService(
        IPlayerStore playerStore, 
        IRankingService rankingService,
        IHubContext<RpsHub> hubContext)
    {
        _playerStore = playerStore;
        _rankingService = rankingService;
        _hubContext = hubContext;
    }

    public async Task<bool> EnqueuePlayerAsync(string playerId, string move)
    {
        if (!Enum.TryParse<GameMove>(move, true, out var gameMove))
        {
            return false;
        }

        var pendingMatch = new PendingMatch
        {
            PlayerId = playerId,
            Move = gameMove,
            Timestamp = DateTime.UtcNow
        };

        _waitingPlayers.Enqueue(pendingMatch);
        
        // Process matches immediately
        await ProcessMatchesAsync();
        
        return true;
    }

    public async Task ProcessMatchesAsync()
    {
        while (_waitingPlayers.Count >= 2)
        {
            if (_waitingPlayers.TryDequeue(out var player1) && 
                _waitingPlayers.TryDequeue(out var player2))
            {
                await ExecuteMatch(player1, player2);
            }
        }
    }

    private async Task ExecuteMatch(PendingMatch player1, PendingMatch player2)
    {
        var result1 = DetermineResult(player1.Move, player2.Move);
        var result2 = DetermineResult(player2.Move, player1.Move);

        var (score1, score2) = CalculateScores(result1);

        // Update scores
        await _playerStore.UpdatePlayerScoreAsync(player1.PlayerId, score1);
        await _playerStore.UpdatePlayerScoreAsync(player2.PlayerId, score2);

        // Get updated player info
        var playerInfo1 = await _playerStore.GetPlayerAsync(player1.PlayerId);
        var playerInfo2 = await _playerStore.GetPlayerAsync(player2.PlayerId);

        if (playerInfo1 == null || playerInfo2 == null)
        {
            return; // Skip if players not found
        }

        // Create round results
        var roundResult1 = new RoundResultDto(
            You: playerInfo1.Name,
            Opponent: playerInfo2.Name,
            YourMove: player1.Move.ToString().ToLowerInvariant(),
            OpponentMove: player2.Move.ToString().ToLowerInvariant(),
            Result: result1.ToString().ToLowerInvariant(),
            DeltaScore: score1
        );

        var roundResult2 = new RoundResultDto(
            You: playerInfo2.Name,
            Opponent: playerInfo1.Name,
            YourMove: player2.Move.ToString().ToLowerInvariant(),
            OpponentMove: player1.Move.ToString().ToLowerInvariant(),
            Result: result2.ToString().ToLowerInvariant(),
            DeltaScore: score2
        );

        // Send results to players
        await _hubContext.Clients.User(player1.PlayerId).SendAsync("RoundResult", roundResult1);
        await _hubContext.Clients.User(player2.PlayerId).SendAsync("RoundResult", roundResult2);

        // Update ranking for all
        await _rankingService.NotifyRankingUpdatedAsync();
        var ranking = await _rankingService.GetRankingAsync();
        await _hubContext.Clients.All.SendAsync("RankingUpdated", ranking);
    }

    private static GameResult DetermineResult(GameMove playerMove, GameMove opponentMove)
    {
        if (playerMove == opponentMove)
            return GameResult.Draw;

        return playerMove switch
        {
            GameMove.Rock when opponentMove == GameMove.Scissors => GameResult.Win,
            GameMove.Paper when opponentMove == GameMove.Rock => GameResult.Win,
            GameMove.Scissors when opponentMove == GameMove.Paper => GameResult.Win,
            _ => GameResult.Lose
        };
    }

    private static (int score1, int score2) CalculateScores(GameResult result1)
    {
        return result1 switch
        {
            GameResult.Win => (WinScore, LoseScore),
            GameResult.Lose => (LoseScore, WinScore),
            GameResult.Draw => (DrawScore, DrawScore),
            _ => (0, 0)
        };
    }
}
