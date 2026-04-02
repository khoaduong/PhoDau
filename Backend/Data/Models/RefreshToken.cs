namespace Backend.Data.Models;

public record RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}