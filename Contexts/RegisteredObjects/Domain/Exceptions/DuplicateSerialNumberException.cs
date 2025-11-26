namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to register an object with a duplicate serial number.
/// </summary>
public sealed class DuplicateSerialNumberException : RegisteredObjectBusinessRuleException
{
    public string SerialNumber { get; }
    public int UserId { get; }
    public int ExistingObjectId { get; }

    public DuplicateSerialNumberException(string serialNumber, int userId, int existingObjectId)
        : base($"An object with serial number '{serialNumber}' already exists")
    {
        SerialNumber = serialNumber;
        UserId = userId;
        ExistingObjectId = existingObjectId;
    }
}