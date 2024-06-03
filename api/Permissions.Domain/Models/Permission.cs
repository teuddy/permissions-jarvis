using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Permissions.Domain.Models;

public class Permission : Entity
{
    [StringLength(100)]
    public string EmployeeForename { get; set; }
    
    [StringLength(100)]
    public string EmployeeSurname { get; set; }
    
    [ForeignKey("PermissionTypeId")]
    public int PermissionTypeId { get; set; }
    
    public virtual PermissionType PermissionType { get; set; } = null!;
    
    public DateTime GrantedOn { get; set; }
}