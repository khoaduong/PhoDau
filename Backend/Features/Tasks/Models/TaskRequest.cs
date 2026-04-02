namespace Backend.Features.Tasks.Models;

public record TaskRequest
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsCompleted { get; init; } = false;
}
