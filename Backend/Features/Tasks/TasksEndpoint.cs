using Backend.Data.Models;
using Backend.Data.Repositories;
using Backend.Features.Tasks.Models;
using Microsoft.AspNetCore.Builder;

namespace Backend.Features.Tasks;

public static class TasksEndpoint
{
    public static void MapTasksEndpoint(this WebApplication app)
    {
        app.MapGet("/api/tasks", GetTasks)
            .WithName("GetTasks")
            .WithOpenApi();

        app.MapGet("/api/tasks/{id:guid}", GetTaskById)
            .WithName("GetTaskById")
            .WithOpenApi();

        app.MapPost("/api/tasks", CreateTask)
            .WithName("CreateTask")
            .WithOpenApi();

        app.MapPut("/api/tasks/{id:guid}", UpdateTask)
            .WithName("UpdateTask")
            .WithOpenApi();

        app.MapDelete("/api/tasks/{id:guid}", DeleteTask)
            .WithName("DeleteTask")
            .WithOpenApi();
    }

    private static async Task<IResult> GetTasks(IRepository<TaskItem> repository)
    {
        var items = await repository.GetAllAsync();
        var response = items.Select(ToResponse);
        return Results.Ok(response);
    }

    private static async Task<IResult> GetTaskById(Guid id, IRepository<TaskItem> repository)
    {
        var item = await repository.GetByIdAsync(id);
        return item is null ? Results.NotFound() : Results.Ok(ToResponse(item));
    }

    private static async Task<IResult> CreateTask(TaskRequest request, IRepository<TaskItem> repository)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest(new { Error = "Title is required." });
        }

        var entity = new TaskItem
        {
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            IsCompleted = request.IsCompleted
        };

        await repository.AddAsync(entity);
        return Results.Created($"/api/tasks/{entity.Id}", ToResponse(entity));
    }

    private static async Task<IResult> UpdateTask(Guid id, TaskRequest request, IRepository<TaskItem> repository)
    {
        var existing = await repository.GetByIdAsync(id);
        if (existing is null)
            return Results.NotFound();

        if (string.IsNullOrWhiteSpace(request.Title))
            return Results.BadRequest(new { Error = "Title is required." });

        existing.Title = request.Title.Trim();
        existing.Description = request.Description?.Trim();
        existing.IsCompleted = request.IsCompleted;
        existing.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(existing);
        return Results.Ok(ToResponse(existing));
    }

    private static async Task<IResult> DeleteTask(Guid id, IRepository<TaskItem> repository)
    {
        var existing = await repository.GetByIdAsync(id);
        if (existing is null)
            return Results.NotFound();

        await repository.DeleteAsync(id);
        return Results.NoContent();
    }

    private static TaskResponse ToResponse(TaskItem item)
        => new TaskResponse
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsCompleted = item.IsCompleted,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
}
