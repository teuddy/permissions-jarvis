namespace Permissions.Domain.Dto;

public class PermissionIndex
{
    public string Id { get; set; }
    public string EmployeeForename { get; set; }
    public string EmployeeSurname { get; set; }
    public string PermissionType { get; set; }
    public string GrantedOn { get; set; }
}