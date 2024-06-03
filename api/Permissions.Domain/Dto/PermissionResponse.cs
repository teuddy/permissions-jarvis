namespace Permissions.Domain.Dto;

public class PermissionResponse
{
    public int Id { get; set; }
    public string EmployeeForename { get; set; }
    public string EmployeeSurname { get; set; }
    public PermissionTypeResponse PermissionType { get; set; }
    public DateTime GrantedOn { get; set; }
}