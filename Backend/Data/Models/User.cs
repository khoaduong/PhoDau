using System.ComponentModel.DataAnnotations;

namespace Backend.Data.Models;

public record User : BaseEntity
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "User";

    public List<RefreshToken> RefreshTokens { get; set; } = new();
}