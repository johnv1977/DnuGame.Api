namespace DnuGame.Api.Common.DTO;

public record PlayerResponse(string Id, string Name, int Score, bool IsOnline, DateTime LastSeen);

public record RankingEntry(string PlayerId, string Name, int Score, bool IsOnline);

public record GameMoveDto(string Move); // "rock" | "paper" | "scissors"

public record RoundResultDto(
    string You,
    string Opponent, 
    string YourMove,
    string OpponentMove,
    string Result, // "win" | "lose" | "draw"
    int DeltaScore);
