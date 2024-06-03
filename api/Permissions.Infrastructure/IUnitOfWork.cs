using Permissions.Domain.Dto;
using Permissions.Domain.Models;
using Permissions.Domain.Repositories;

namespace Permissions.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    IRepository<Permission> PermissionsRepository { get; }
    IRepository<PermissionType> PermissionTypesRepository { get; }
    Task Commit();
    Task EmitEvent(EventType type);
    Task SyncWithElastic(Permission newPermission);
}