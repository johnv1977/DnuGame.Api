using Microsoft.AspNetCore.Identity;

namespace DnuGame.Api.Infrastructure.Auth;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
}
