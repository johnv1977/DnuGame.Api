using System.Text;
using DnuGame.Api.Common.Abstractions;
using DnuGame.Api.Infrastructure.Auth;
using DnuGame.Api.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DnuGame.Api.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("Default") ?? "Data Source=app.db";
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(conn));

        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager();

        var jwtSection = config.GetSection("JWT");
        var key = jwtSection.GetValue<string>("Key") ?? "dev-secret-key-change";
        var issuer = jwtSection.GetValue<string>("Issuer") ?? "DnuGame";
        var audience = jwtSection.GetValue<string>("Audience") ?? "DnuGameClients";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.FromMinutes(1)
            };
            // Permitir tokens via query para WebSockets (SignalR)
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = ctx =>
                {
                    var accessToken = ctx.Request.Query["access_token"].ToString();
                    var path = ctx.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        ctx.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Admin");
            });
        });
        
        // SignalR
        services.AddSignalR();
        
        // CORS
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(
                    "http://localhost:3000", // React dev
                    "http://localhost:5173", // Vite dev
                    "https://localhost:3000",
                    "https://localhost:5173"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });
        
        // Servicios de aplicación
        services.AddScoped<ITokenService, TokenService>();
        services.AddSingleton<IPlayerStore, DnuGame.Api.Modules.Players.InMemoryPlayerStore>();
        services.AddSingleton<IRankingService, DnuGame.Api.Modules.Ranking.RankingService>();
        services.AddSingleton<IGameRpsService, DnuGame.Api.Modules.GameRps.GameRpsService>();
        
        // Servicios del módulo Rooms
        services.AddScoped<DnuGame.Api.Modules.Rooms.Repositories.IRoomRepository, DnuGame.Api.Modules.Rooms.Repositories.RoomRepository>();
        services.AddScoped<DnuGame.Api.Modules.Rooms.Services.IRoomService, DnuGame.Api.Modules.Rooms.Services.RoomService>();
        
        return services;
    }
}
