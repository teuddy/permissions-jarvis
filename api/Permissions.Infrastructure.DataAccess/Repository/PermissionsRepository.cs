using Microsoft.EntityFrameworkCore;
using Permissions.Domain.Models;
using Permissions.Domain.Repositories;

namespace Permissions.Infrastructure.DataAccess.Repository;

public class PermissionsRepository : IRepository<Permission>
{
    private readonly PermissionsContext _context;

    public PermissionsRepository(PermissionsContext context)
    {
        _context = context;
    }

    public async Task<int> Add(Permission entity)
    {
         var result = await _context.Permissions.AddAsync(entity);
         return result.Entity.Id;
    }

    public void Update(Permission entity)
    {
       _context.Permissions.Update(entity);
    }

    public Task<List<Permission>> GetAll()
    {
        return _context.Permissions
            .Include(p => p.PermissionType)
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();
    }

    public Task<Permission?> GetById(int id)
    {
        return _context.Permissions
            .Include(p => p.PermissionType)
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<bool> Exists(int id)
    {
        return _context.Permissions.AnyAsync(p => p.Id == id);
    }
}