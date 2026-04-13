namespace Backend.Features.Tasks;

public static class TasksEndpoints
{
    public static void MapTasksEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tasks")
            .RequireAuthorization("UserOnly");

        group.MapGet("/", async (TasksService service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        });

        group.MapGet("/{id:guid}", async (Guid id, TasksService service) =>
        {
            var task = await service.GetByIdAsync(id);
            return task is null ? Results.NotFound() : Results.Ok(task);
        });

        group.MapPost("/", async (CreateTaskRequest request, TasksService service) =>
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return Results.BadRequest(new ErrorResponse("Validation failed", new { Title = "Title is required" }));

            if (request.Title.Length > 200)
                return Results.BadRequest(new ErrorResponse("Validation failed", new { Title = "Title must be 200 characters or less" }));

            var task = await service.CreateAsync(request.Title);
            return Results.Created($"/api/tasks/{task.Id}", task);
        });

        group.MapPut("/{id:guid}/toggle", async (Guid id, TasksService service) =>
        {
            var success = await service.ToggleAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, TasksService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization("AdminOnly");
    }

    public record CreateTaskRequest(string Title);
}
