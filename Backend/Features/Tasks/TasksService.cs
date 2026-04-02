using Backend.Data;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Tasks;

public class TasksService
{
    private readonly AppDbContext _db;

    public TasksService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _db.Tasks
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _db.Tasks.FindAsync(id);
    }

    public async Task<TaskItem> CreateAsync(string title)
    {
        var task = new TaskItem
        {
            Title = title,
            IsCompleted = false
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        return task;
    }

    public async Task<bool> ToggleAsync(Guid id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return false;

        task.IsCompleted = !task.IsCompleted;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task == null) return false;

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();

        return true;
    }
}