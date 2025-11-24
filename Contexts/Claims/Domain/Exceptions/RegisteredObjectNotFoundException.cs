// Contexts/Claims/Domain/Exceptions/RegisteredObjectNotFoundException.cs
namespace learning_center_webapi.Contexts.Claims.Domain.Exceptions;

/// <summary>
/// Exception thrown when a registered object with the specified ID is not found.
/// </summary>
public class RegisteredObjectNotFoundException : Exception
{
    public int RegisteredObjectId { get; }

    public RegisteredObjectNotFoundException(int registeredObjectId)
        : base($"Registered object with ID {registeredObjectId} not found")
    {
        RegisteredObjectId = registeredObjectId;
    }
}