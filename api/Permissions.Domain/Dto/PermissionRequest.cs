using System.ComponentModel.DataAnnotations;

namespace Permissions.Domain.Dto;

public class PermissionRequest
{
    [Required]
    public string EmployeeForename { get; set; }
    
    [Required]
    public string EmployeeSurname { get; set; }
    
    [Required]
    public int PermissionType { get; set; }
}