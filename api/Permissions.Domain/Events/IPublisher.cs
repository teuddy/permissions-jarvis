using Permissions.Domain.Models;

namespace Permissions.Domain.Events;

public interface IPublisher<in T> where T : class
{
    Task PublishMessageAsync(T message);
}