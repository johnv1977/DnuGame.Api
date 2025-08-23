using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DnuGame.Api.Common.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace DnuGame.Api.Infrastructure.Auth;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public Task<string> CreateTokenAsync(string userId, string username, string? displayName = null)
    {
        var jwtSection = _config.GetSection("JWT");
        var key = jwtSection.GetValue<string>("Key") ?? "dev-secret-key-change";
        var issuer = jwtSection.GetValue<string>("Issuer") ?? "DnuGame";
        var audience = jwtSection.GetValue<string>("Audience") ?? "DnuGameClients";
        var expiresMinutes = jwtSection.GetValue<int>("ExpiresMinutes", 60);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, username),
            new("unique_name", username),
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        if (!string.IsNullOrEmpty(displayName))
        {
            claims.Add(new("display_name", displayName));
        }

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: creds
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
