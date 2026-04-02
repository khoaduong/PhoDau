using Backend.Data.Models;

namespace Backend.Data.Repositories;

public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly List<T> _data = new();

    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult(_data.AsEnumerable());
    }

    public Task<T?> GetByIdAsync(Guid id)
    {
        var entity = _data.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(entity);
    }

    public Task AddAsync(T entity)
    {
        _data.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity)
    {
        var existing = _data.FirstOrDefault(x => x.Id == entity.Id);
        if (existing != null)
        {
            _data.Remove(existing);
            _data.Add(entity);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var entity = _data.FirstOrDefault(x => x.Id == id);
        if (entity != null)
        {
            _data.Remove(entity);
        }
        return Task.CompletedTask;
    }
}
