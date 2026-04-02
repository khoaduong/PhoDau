namespace Backend.Features.Tasks;

public record ErrorResponse(string Message, object? Errors = null);
