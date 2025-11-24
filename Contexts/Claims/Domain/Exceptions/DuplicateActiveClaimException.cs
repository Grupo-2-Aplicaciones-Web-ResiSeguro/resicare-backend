// Contexts/Claims/Domain/Exceptions/DuplicateActiveClaimException.cs
namespace learning_center_webapi.Contexts.Claims.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to create a claim for a registered object that already has an active claim.
/// </summary>
public class DuplicateActiveClaimException : Exception
{
    public int RegisteredObjectId { get; }
    public int ExistingClaimId { get; }

    public DuplicateActiveClaimException(int registeredObjectId, int existingClaimId)
        : base($"Active claim already exists for registered object {registeredObjectId}")
    {
        RegisteredObjectId = registeredObjectId;
        ExistingClaimId = existingClaimId;
    }
}