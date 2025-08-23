namespace DnuGame.Api.Modules.Players;

public class PlayerState
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }
    public bool IsOnline { get; set; }
    public DateTime LastSeen { get; set; }
    public string? ConnectionId { get; set; }
}

public enum GameMove
{
    Rock,
    Paper,
    Scissors
}

public enum GameResult
{
    Win,
    Lose,
    Draw
}

public class PendingMatch
{
    public string PlayerId { get; set; } = string.Empty;
    public GameMove Move { get; set; }
    public DateTime Timestamp { get; set; }
}
