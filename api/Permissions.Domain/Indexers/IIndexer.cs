using Permissions.Domain.Models;

namespace Permissions.Domain.Indexers;

public interface IIndexer<in T> where T : class
{
    Task SyncToIndexAsync(T entity);
}