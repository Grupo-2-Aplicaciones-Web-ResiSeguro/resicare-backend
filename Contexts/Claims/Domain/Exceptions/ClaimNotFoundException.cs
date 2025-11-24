// Contexts/Claims/Domain/Exceptions/ClaimNotFoundException.cs
namespace learning_center_webapi.Contexts.Claims.Domain.Exceptions;

/// <summary>
/// Exception thrown when a claim with the specified ID is not found.
/// </summary>
public class ClaimNotFoundException : Exception
{
    public int ClaimId { get; }

    public ClaimNotFoundException(int claimId)
        : base($"Claim with ID {claimId} not found")
    {
        ClaimId = claimId;
    }
}