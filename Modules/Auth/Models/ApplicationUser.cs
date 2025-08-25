using Microsoft.AspNetCore.Identity;

namespace DnuGame.Api.Modules.Auth.Models;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
}
