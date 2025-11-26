namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to modify or delete an object with active claims.
/// </summary>
public sealed class CannotModifyObjectWithActiveClaimException : RegisteredObjectBusinessRuleException
{
    public int ObjectId { get; }
    public int ActiveClaimId { get; }

    public CannotModifyObjectWithActiveClaimException(int objectId, int activeClaimId)
        : base("Cannot modify object with active claims")
    {
        ObjectId = objectId;
        ActiveClaimId = activeClaimId;
    }
}