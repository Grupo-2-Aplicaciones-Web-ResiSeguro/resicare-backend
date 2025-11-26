namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;

/// <summary>
/// Exception thrown when a registered object with the specified ID is not found.
/// </summary>
public sealed class RegisteredObjectNotFoundException : RegisteredObjectBusinessRuleException
{
    public int ObjectId { get; }

    public RegisteredObjectNotFoundException(int objectId)
        : base($"Registered object with ID {objectId} not found")
    {
        ObjectId = objectId;
    }
}