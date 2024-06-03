using Microsoft.EntityFrameworkCore;
using Permissions.Domain.Models;

namespace Permissions.Infrastructure.DataAccess;

public class PermissionsContext : DbContext
{
    public PermissionsContext(DbContextOptions<PermissionsContext> options) : base(options)
    {
    }
    
    public DbSet<PermissionType> PermissionTypes { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
}