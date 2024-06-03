using Permissions.Domain.Dto;
using Permissions.Domain.Events;
using Permissions.Domain.Indexers;
using Permissions.Domain.Models;
using Permissions.Domain.Repositories;
using Permissions.Infrastructure.DataAccess;

namespace Permissions.Infrastructure;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    public IRepository<Permission> PermissionsRepository { get; }
    public IRepository<PermissionType> PermissionTypesRepository { get; }

    private readonly IPublisher<PermissionEvent> _publisher;
    private readonly IIndexer<PermissionIndex> _indexer;
    
    private readonly PermissionsContext _context;

    public UnitOfWork(PermissionsContext context, 
        IRepository<Permission> permissionsRepository, 
        IRepository<PermissionType> permissionTypesRepository, 
        IPublisher<PermissionEvent> publisher, 
        IIndexer<PermissionIndex> indexer)
    {
        _context = context;
        PermissionsRepository = permissionsRepository;
        PermissionTypesRepository = permissionTypesRepository;
        _publisher = publisher;
        _indexer = indexer;
    }
    
    public Task Commit()
    {
        return _context.SaveChangesAsync();
    }

    public Task EmitEvent(EventType type)
    {
        var id = Guid.NewGuid();
        var permissionEvent = new PermissionEvent(id, type);

        return _publisher.PublishMessageAsync(permissionEvent);
    }

    public Task SyncWithElastic(Permission newPermission)
    {
        var permissionIndex = new PermissionIndex
        {
            Id = newPermission.Id.ToString(),
            EmployeeForename = newPermission.EmployeeForename,
            EmployeeSurname = newPermission.EmployeeSurname,
            PermissionType = newPermission.PermissionType.Description,
            GrantedOn = newPermission.GrantedOn.ToString("O")
        };
        
        return _indexer.SyncToIndexAsync(permissionIndex);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _context.DisposeAsync();
    }
}