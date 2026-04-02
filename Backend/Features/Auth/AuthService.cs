using System.Security.Cryptography;
using Backend.Data;
using Backend.Data.Models;
using Backend.Features.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Auth;

public class AuthService
{
    private readonly AppDbContext _dbContext;
    private readonly JwtTokenProvider _tokenProvider;
    private readonly PasswordHasher<User> _passwordHasher;

    private const int RefreshTokenDays = 30;

    public AuthService(AppDbContext dbContext, JwtTokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<(bool Success, LoginResponse? Response, string? ErrorMessage)> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return (false, null, "Username and password are required");

        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return (false, null, "Invalid credentials");

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return (false, null, "Invalid credentials");

        var token = _tokenProvider.GenerateToken(user.Username, user.Role);
        var refreshToken = CreateRefreshToken(user);

        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();

        return (true, new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken.Token,
            Username = user.Username,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        }, null);
    }

    public async Task<(bool Success, RefreshResponse? Response, string? ErrorMessage)> RefreshTokenAsync(string token, string refreshToken)
    {
        var principal = _tokenProvider.GetPrincipalFromExpiredToken(token);
        if (principal == null)
            return (false, null, "Invalid access token");

        var username = principal.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username))
            return (false, null, "Invalid token");

        var user = await _dbContext.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.Username == username);
        if (user == null)
            return (false, null, "Invalid token");

        var storedRefreshToken = user.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken);
        if (storedRefreshToken == null || !storedRefreshToken.IsActive)
            return (false, null, "Invalid refresh token");

        storedRefreshToken.RevokedAt = DateTime.UtcNow;

        var newAccessToken = _tokenProvider.GenerateToken(user.Username, user.Role);
        var newRefreshToken = CreateRefreshToken(user);

        _dbContext.RefreshTokens.Add(newRefreshToken);
        await _dbContext.SaveChangesAsync();

        return (true, new RefreshResponse
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        }, null);
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
    {
        var tokenEntity = await _dbContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == refreshToken);
        if (tokenEntity == null || !tokenEntity.IsActive)
            return false;

        tokenEntity.RevokedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User> CreateUserAsync(string username, string password, string role = "User")
    {
        var existingUser = await _dbContext.Users.AnyAsync(u => u.Username == username);
        if (existingUser)
            throw new InvalidOperationException("Username already exists.");

        var user = new User
        {
            Username = username,
            Role = role
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    private RefreshToken CreateRefreshToken(User user)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = DateTime.UtcNow.AddDays(RefreshTokenDays),
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id,
            User = user
        };
    }
}
