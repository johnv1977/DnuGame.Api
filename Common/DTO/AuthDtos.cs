using System.ComponentModel.DataAnnotations;

namespace DnuGame.Api.Common.DTO;

public class AuthRegisterRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Display name cannot exceed 100 characters")]
    public string? DisplayName { get; set; }
}

public class AuthLoginRequest
{
    [Required(ErrorMessage = "Username or Email is required")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public record AuthTokenResponse(string AccessToken, DateTime ExpiresAt);

public record MeResponse(string Id, string Username, string? Email, string? DisplayName);

public record ErrorResponse(string Type, string Title, string Detail, int Status);
