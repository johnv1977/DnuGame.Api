using System.ComponentModel.DataAnnotations;

namespace DnuGame.Api.Modules.Rooms.DTOs;

public class CreateRoomDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug is required")]
    [StringLength(50, ErrorMessage = "Slug cannot exceed 50 characters")]
    [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "Slug can only contain alphanumeric characters and hyphens")]
    public string Slug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Color is required")]
    [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Color must be a valid hex format (#RRGGBB)")]
    public string Color { get; set; } = string.Empty;

    [Required(ErrorMessage = "Icon is required")]
    [StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters")]
    public string Icon { get; set; } = string.Empty;

    [Required(ErrorMessage = "UserLimit is required")]
    [Range(2, 50, ErrorMessage = "UserLimit must be between 2 and 50")]
    public int UserLimit { get; set; }

    public bool IsOpen { get; set; } = true;
}

public class UpdateRoomDto
{
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string? Name { get; set; }

    [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Color must be a valid hex format (#RRGGBB)")]
    public string? Color { get; set; }

    [StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters")]
    public string? Icon { get; set; }

    [Range(2, 50, ErrorMessage = "UserLimit must be between 2 and 50")]
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
