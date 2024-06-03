using System.Diagnostics.CodeAnalysis;
using Mediator;

namespace Permissions.Api.Validation;

public interface IValidate : IMessage
{
    bool IsValid([NotNullWhen(false)] out ValidationError? error);
}
public record ValidationResponse(string PropertyName, string Message);

public record ValidationError(List<ValidationResponse> Errors);

public class ValidationException : Exception
{
    public ValidationException(ValidationError error) : base("Validation error")
    {
        Error = error;
    }

    public ValidationError Error { get; }
}

public class PermissionException : Exception
{
    public PermissionException(string message) : base(message)
    {
    }
}
