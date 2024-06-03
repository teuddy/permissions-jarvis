using FluentValidation;
using Mediator;
using Permissions.Api.Validation;
using Permissions.Domain.Dto;
using Permissions.Infrastructure;

namespace Permissions.Api.Handlers.Commands;

public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(x => x.EmployeeForename).NotEmpty();
        RuleFor(x => x.EmployeeSurname).NotEmpty();
        RuleFor(x => x.PermissionType).NotEmpty().GreaterThan(0);
    }
}

public record UpdatePermissionCommand
    (int Id, string EmployeeForename, string EmployeeSurname, int PermissionType) : ICommand, IValidate
{
    public bool IsValid(out ValidationError? error)
    {
        var validator = new UpdatePermissionCommandValidator();
        var result = validator.Validate(this);
        if (result.IsValid)
            error = null;
        else
            error = new ValidationError(result.Errors.Select(e => new ValidationResponse(e.PropertyName, e.ErrorMessage)).ToList());

        return result.IsValid;
    }
}

public class UpdatePermissionCommandHandler : ICommandHandler<UpdatePermissionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePermissionCommandHandler> _logger;
    
    public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePermissionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async ValueTask<Unit> Handle(UpdatePermissionCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating permission with id {Id}", command.Id);
        
        var permission = await _unitOfWork.PermissionsRepository.GetById(command.Id);
        
        if (permission == null)
        {
            _logger.LogWarning("Permission with id {Id} not found", command.Id);
            throw new PermissionException($"Permission with id {command.Id} not found");
        }
        
        var permissionType = await _unitOfWork.PermissionTypesRepository.GetById(command.PermissionType);
        
        if (permissionType == null)
        {
            _logger.LogWarning("Permission type with id {Id} not found", command.PermissionType);
            throw new PermissionException($"Permission type with id {command.PermissionType} not found");
        }
        
        permission.EmployeeForename = command.EmployeeForename;
        permission.EmployeeSurname = command.EmployeeSurname;
        permission.PermissionType = permissionType;
        
        _unitOfWork.PermissionsRepository.Update(permission);
        await _unitOfWork.Commit();
        
        _logger.LogInformation("Permission with id {Id} updated", command.Id);
        
        _logger.LogInformation("Emitting event {EventType}", "Modify");
        await _unitOfWork.EmitEvent(EventType.Modify);
        
        _logger.LogInformation("Syncing with elastic");
        await _unitOfWork.SyncWithElastic(permission);
        
        return Unit.Value;
    }
}