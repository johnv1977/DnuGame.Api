using DnuGame.Api.Modules.Auth.Models;
using DnuGame.Api.Modules.Rooms.Configuration;
using DnuGame.Api.Modules.Rooms.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DnuGame.Api.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Aplicar configuraciones de entidades
        builder.ApplyConfiguration(new RoomConfiguration());
    }
}
