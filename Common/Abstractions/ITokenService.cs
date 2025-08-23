namespace DnuGame.Api.Common.Abstractions;

public interface ITokenService
{
    Task<string> CreateTokenAsync(string userId, string username, string? displayName = null);
}
