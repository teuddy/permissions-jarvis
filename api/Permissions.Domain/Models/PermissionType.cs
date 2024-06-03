using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Permissions.Domain.Models;

public class PermissionType : Entity
{
    [StringLength(250)]
    public string Description { get; set; }
}