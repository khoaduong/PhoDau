using Backend.Features.Auth.Models;

namespace Backend.Features.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", Login)
            .WithName("Login")
            .WithOpenApi()
            .AllowAnonymous();

        group.MapPost("/refresh", RefreshToken)
            .WithName("RefreshToken")
            .WithOpenApi()
            .AllowAnonymous();

        group.MapPost("/logout", Logout)
            .WithName("Logout")
            .WithOpenApi()
            .RequireAuthorization();
    }

    private static async Task<IResult> Login(LoginRequest request, AuthService authService)
    {
        var (success, response, errorMessage) = await authService.LoginAsync(request.Username, request.Password);
        if (!success || response is null)
            return Results.Unauthorized();

        return Results.Ok(response);
    }

    private static async Task<IResult> RefreshToken(RefreshRequest request, AuthService authService)
    {
        var (success, response, errorMessage) = await authService.RefreshTokenAsync(request.Token, request.RefreshToken);

        if (!success || response is null)
            return Results.Unauthorized();

        return Results.Ok(response);
    }

    private static async Task<IResult> Logout(RefreshRequest request, AuthService authService)
    {
        var revoked = await authService.RevokeRefreshTokenAsync(request.RefreshToken);
        if (!revoked) return Results.BadRequest(new { Error = "Unable to revoke refresh token" });

        return Results.NoContent();
    }
}

