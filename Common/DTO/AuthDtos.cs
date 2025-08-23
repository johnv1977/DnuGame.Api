namespace DnuGame.Api.Common.DTO;

public record AuthRegisterRequest(string Username, string Email, string Password, string? DisplayName = null);

public record AuthLoginRequest(string UsernameOrEmail, string Password);

public record AuthTokenResponse(string AccessToken, DateTime ExpiresAt);

public record MeResponse(string Id, string Username, string? Email, string? DisplayName);

public record ErrorResponse(string Type, string Title, string Detail, int Status);
