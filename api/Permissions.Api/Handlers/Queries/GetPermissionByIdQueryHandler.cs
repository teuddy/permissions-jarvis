using Mediator;
using Permissions.Domain.Dto;
using Permissions.Domain.Models;
using Permissions.Infrastructure;

namespace Permissions.Api.Handlers.Queries;

public record GetPermissionByIdQuery(int Id) : IQuery<PermissionResponse>;

public class GetPermissionByIdQueryHandler : IQueryHandler<GetPermissionByIdQuery, PermissionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public GetPermissionByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<PermissionResponse> Handle(GetPermissionByIdQuery query, CancellationToken cancellationToken)
    {
        var response = await _unitOfWork.PermissionsRepository.GetById(query.Id);
        
        if (response == null)
        {
            return null;
        }

        return new PermissionResponse
        {
            Id = response.Id,
            EmployeeForename = response.EmployeeForename,
            EmployeeSurname = response.EmployeeSurname,
            PermissionType = new PermissionTypeResponse
            {
                Id = response.PermissionType.Id,
                Description = response.PermissionType.Description
            },
            GrantedOn = response.GrantedOn
        };
    }
}