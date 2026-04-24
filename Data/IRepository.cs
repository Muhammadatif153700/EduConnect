namespace EduConnect.Data;

// SOLID: Interface Segregation Principle (ISP) - Keep repository interface focused on basic CRUD
// SOLID: Open/Closed Principle (OCP) - Allows creating different repository implementations without changing the interfaces
public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T? GetById(Guid id);
    void Add(T entity);
    void Update(T entity);
    void Delete(Guid id);
    IEnumerable<T> Find(Func<T, bool> predicate);
}
