using Microsoft.EntityFrameworkCore;
using Permissions.Domain.Models;
using Permissions.Domain.Repositories;

namespace Permissions.Infrastructure.DataAccess.Repository;

public class PermissionTypeRepository : IRepository<PermissionType>
{
    private readonly PermissionsContext _context;
    
    public PermissionTypeRepository(PermissionsContext context)
    {
        _context = context;
    }
    
    public async Task<int> Add(PermissionType entity)
    {
        var result = await _context.PermissionTypes.AddAsync(entity);
        
        return result.Entity.Id;
    }

    public void Update(PermissionType entity)
    {
        _context.PermissionTypes.Update(entity);
    }

    public Task<List<PermissionType>> GetAll()
    {
        return _context.PermissionTypes
            .AsNoTrackingWithIdentityResolution()
            .ToListAsync();
    }

    public Task<PermissionType?> GetById(int id)
    {
        return _context.PermissionTypes
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task<bool> Exists(int id)
    {
        return _context.PermissionTypes.AnyAsync(p => p.Id == id);
    }
}