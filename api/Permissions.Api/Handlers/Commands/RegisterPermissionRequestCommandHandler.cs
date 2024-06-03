using FluentValidation;
using Mediator;
using Permissions.Api.Validation;
using Permissions.Domain.Dto;
using Permissions.Domain.Models;
using Permissions.Infrastructure;

namespace Permissions.Api.Handlers.Commands;

public class RegisterPermissionRequestValidator : AbstractValidator<RegisterPermissionRequestCommand>
{
    public RegisterPermissionRequestValidator()
    {
        RuleFor(x => x.EmployeeForename).NotEmpty();
        RuleFor(x => x.EmployeeSurname).NotEmpty();
        RuleFor(x => x.PermissionType).NotEmpty();
        RuleFor(x => x.PermissionType).GreaterThan(0);
    }
}

public record RegisterPermissionRequestCommand
    (string EmployeeForename, string EmployeeSurname, int PermissionType) : ICommand<Permission>, IValidate
{
    public bool IsValid(out ValidationError? error)
    {
        var validator = new RegisterPermissionRequestValidator();
        var result = validator.Validate(this);
        if (result.IsValid)
            error = null;
        else
            error = new ValidationError(result.Errors.Select(e => new ValidationResponse(e.PropertyName, e.ErrorMessage)).ToList());

        return result.IsValid;
    }
}

public class RegisterPermissionRequestCommandHandler : ICommandHandler<RegisterPermissionRequestCommand, Permission>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterPermissionRequestCommandHandler> _logger;
    
    public RegisterPermissionRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<RegisterPermissionRequestCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async ValueTask<Permission> Handle(RegisterPermissionRequestCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering new permission request");
        
        var permissionType = await _unitOfWork.PermissionTypesRepository.GetById(command.PermissionType);
        
        if (permissionType == null)
        {
            _logger.LogWarning("Permission type with id {Id} not found", command.PermissionType);
            throw new PermissionException($"Permission type with id {command.PermissionType} not found");
        }
        
        var newPermission = new Permission
        {
            EmployeeForename = command.EmployeeForename,
            EmployeeSurname = command.EmployeeSurname,
            PermissionTypeId = command.PermissionType,
            GrantedOn = DateTime.UtcNow
        };
        
        var newId = await _unitOfWork.PermissionsRepository.Add(newPermission);
        await _unitOfWork.Commit();
        
        newPermission.Id = newId;
        
        _logger.LogInformation("New permission request registered with id {Id}", newId);
        
        _logger.LogInformation("Emitting event");
        await _unitOfWork.EmitEvent(EventType.Request);
        
        _logger.LogInformation("Syncing with elastic");
        await _unitOfWork.SyncWithElastic(newPermission);
        
        return newPermission;
    }
}