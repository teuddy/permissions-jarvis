using Mediator;
using Permissions.Domain.Dto;
using Permissions.Domain.Models;
using Permissions.Infrastructure;

namespace Permissions.Api.Handlers.Queries;

public record GetAllPermissionsQuery : IQuery<IEnumerable<PermissionResponse>>;

public class GetAllPermissionsQueryHandler : IQueryHandler<GetAllPermissionsQuery, IEnumerable<PermissionResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPermissionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<IEnumerable<PermissionResponse>> Handle(GetAllPermissionsQuery query, CancellationToken cancellationToken)
    {
        var allPermissions = await _unitOfWork.PermissionsRepository.GetAll();

        await _unitOfWork.EmitEvent(EventType.Get);
        
        return allPermissions.Select(p => new PermissionResponse
        {
            Id = p.Id,
            EmployeeForename = p.EmployeeForename,
            EmployeeSurname = p.EmployeeSurname,
            PermissionType = new PermissionTypeResponse
            {
                Id = p.PermissionType.Id,
                Description = p.PermissionType.Description
            },
            GrantedOn = p.GrantedOn
        });
    }
}