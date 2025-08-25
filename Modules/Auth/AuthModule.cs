using DnuGame.Api.Common.Abstractions;
using DnuGame.Api.Common.DTO;
using DnuGame.Api.Modules.Auth.DTOs;
using DnuGame.Api.Modules.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DnuGame.Api.Modules.Auth;

public static class AuthModule
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest);

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithOpenApi()
            .Produces<AuthTokenResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/me", GetMeAsync)
            .RequireAuthorization()
            .WithName("GetMe")
            .WithOpenApi()
            .Produces<MeResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }

    private static async Task<IResult> RegisterAsync(
        [FromBody] AuthRegisterRequest request,
        UserManager<ApplicationUser> userManager)
    {
        // La validación se hace automáticamente por DataAnnotations
        
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            DisplayName = request.DisplayName
        };

        var result = await userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Results.BadRequest(new ErrorResponse(
                "registration_failed",
                "Registration Failed",
                errors,
                400));
        }

        return Results.Ok(new { message = "User registered successfully", userId = user.Id });
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] AuthLoginRequest request,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        // La validación se hace automáticamente por DataAnnotations
        
        var user = await userManager.FindByNameAsync(request.UsernameOrEmail) ??
                   await userManager.FindByEmailAsync(request.UsernameOrEmail);

        if (user == null)
        {
            return Results.Unauthorized();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        
        if (!result.Succeeded)
        {
            return Results.Unauthorized();
        }

        var token = await tokenService.CreateTokenAsync(user.Id, user.UserName!, user.DisplayName);
        var expiresAt = DateTime.UtcNow.AddMinutes(60); // TODO: from config

        return Results.Ok(new AuthTokenResponse(token, expiresAt, user));
    }

    private static async Task<IResult> GetMeAsync(
        HttpContext context,
        UserManager<ApplicationUser> userManager)
    {
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }

        var user = await userManager.FindByIdAsync(userId);
        
        if (user == null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(new MeResponse(user.Id, user.UserName!, user.Email, user.DisplayName));
    }
}
