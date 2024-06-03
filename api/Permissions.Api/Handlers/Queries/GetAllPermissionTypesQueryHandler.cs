using Mediator;
using Permissions.Domain.Dto;
using Permissions.Infrastructure;

namespace Permissions.Api.Handlers.Queries;

public record GetAllPermissionTypesQuery() : IQuery<IEnumerable<PermissionTypeResponse>>;

public class GetAllPermissionTypesQueryHandler : IQueryHandler<GetAllPermissionTypesQuery, IEnumerable<PermissionTypeResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPermissionTypesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<IEnumerable<PermissionTypeResponse>> Handle(GetAllPermissionTypesQuery query, CancellationToken cancellationToken)
    {
        var response = await _unitOfWork.PermissionTypesRepository.GetAll();
        var result = response.Select(x => new PermissionTypeResponse
        {
            Id = x.Id,
            Description = x.Description
        });

        return result;
    }
}