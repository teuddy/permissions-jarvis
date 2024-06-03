using Permissions.Domain.Models;

namespace Permissions.Domain.Repositories;

public interface IRepository<T> where T : Entity
{
    Task<int> Add(T entity);
    void Update(T entity);
    Task<List<T>> GetAll();
    Task<T?> GetById(int id);
    Task<bool> Exists(int id);
}