using System.Runtime.Serialization;

namespace Permissions.Domain.Dto;

public record PermissionEvent(Guid Id, EventType EventType);

public enum EventType
{
    [EnumMember(Value = "request")]
    Request,
    [EnumMember(Value = "modify")]
    Modify,
    [EnumMember(Value = "get")]
    Get
}