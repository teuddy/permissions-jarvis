using Mediator;

namespace Permissions.Api.Validation;

public class MessageValidatorBehaviour<TMessage, TResponse> : MessagePreProcessor<TMessage, TResponse> where TMessage : IValidate
{
    protected override ValueTask Handle(TMessage message, CancellationToken cancellationToken)
    {
        if (!message.IsValid(out var error))
        {
            throw new ValidationException(error);
        }

        return ValueTask.CompletedTask;
    }
}