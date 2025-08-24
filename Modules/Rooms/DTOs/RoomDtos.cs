namespace DnuGame.Api.Modules.Rooms.DTOs;

public class CreateRoomDto
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int UserLimit { get; set; }
    public bool IsOpen { get; set; } = true;
}

public class UpdateRoomDto
{
    public string? Name { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public int? UserLimit { get; set; }
    public bool? IsOpen { get; set; }
}

public record RoomResponseDto(
    Guid Id,
    string Slug,
    string Name,
    string Color,
    string Icon,
    int UserLimit,
    bool IsOpen,
    int CurrentUserCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record RoomListDto(
    Guid Id,
    string Slug,
    string Name,
    string Color,
    string Icon,
    int UserLimit,
    bool IsOpen,
    int CurrentUserCount
);

public record PaginatedResult<T>(
    IEnumerable<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);
