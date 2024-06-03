using FluentValidation;
using Mediator;
using Permissions.Api.Validation;
using Permissions.Domain.Models;
using Permissions.Infrastructure;

namespace Permissions.Api.Handlers.Commands;

public class CreatePermissionTypeValidator : AbstractValidator<CreatePermissionTypeCommand>
{
    public CreatePermissionTypeValidator()
    {
        RuleFor(x => x.Description).NotEmpty();
    }
}

public record CreatePermissionTypeCommand(string Description) : ICommand, IValidate
{
    public bool IsValid(out ValidationError? error)
    {
        var validator = new CreatePermissionTypeValidator();
        var result = validator.Validate(this);
        if (result.IsValid)
            error = null;
        else
            error = new ValidationError(result.Errors.Select(e => new ValidationResponse(e.PropertyName, e.ErrorMessage)).ToList());

        return result.IsValid;
    }
}

public class CreatePermissionTypeCommandHandler : ICommandHandler<CreatePermissionTypeCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePermissionTypeCommandHandler> _logger;
    
    public CreatePermissionTypeCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePermissionTypeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async ValueTask<Unit> Handle(CreatePermissionTypeCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new permission type");
        var newPermissionType = new PermissionType
        {
            Description = command.Description
        };
        
        var newId = await _unitOfWork.PermissionTypesRepository.Add(newPermissionType);
        await _unitOfWork.Commit();
        
        _logger.LogInformation("New permission type created with id {Id}", newId);
        
        return Unit.Value;
    }
}