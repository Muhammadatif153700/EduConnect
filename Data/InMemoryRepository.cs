using System.Reflection;

namespace EduConnect.Data;

// SOLID: Liskov Substitution Principle (LSP) - Can be used wherever IRepository is expected.
public class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> _data = new();

    public IEnumerable<T> GetAll() => _data;

    public T? GetById(Guid id)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return null;
        
        return _data.FirstOrDefault(e => (Guid)propertyInfo.GetValue(e)! == id);
    }

    public void Add(T entity)
    {
        _data.Add(entity);
    }

    public void Update(T entity)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return;
        
        var id = (Guid)propertyInfo.GetValue(entity)!;
        var existing = GetById(id);
        
        if (existing != null)
        {
            var index = _data.IndexOf(existing);
            _data[index] = entity;
        }
    }

    public void Delete(Guid id)
    {
        var existing = GetById(id);
        if (existing != null)
        {
            _data.Remove(existing);
        }
    }

    public IEnumerable<T> Find(Func<T, bool> predicate)
    {
        return _data.Where(predicate);
    }
}
