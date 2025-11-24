// Contexts/Claims/Application/CommandServices/IClaimCommandService.cs
using learning_center_webapi.Contexts.Claims.Domain.Commands;
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Claims.Application.CommandServices;

/// <summary>
/// Interface for claim command operations.
/// </summary>
public interface IClaimCommandService
{
    /// <summary>
    /// Handles the creation of a new claim.
    /// </summary>
    /// <param name="command">Command containing claim creation data.</param>
    /// <returns>The created claim entity.</returns>
    Task<Claim> Handle(CreateClaimCommand command);

    /// <summary>
    /// Handles the update of an existing claim.
    /// </summary>
    /// <param name="command">Command containing claim update data.</param>
    /// <returns>The updated claim entity, or null if not found.</returns>
    Task<Claim?> Handle(UpdateClaimCommand command);

    /// <summary>
    /// Deletes a claim by its identifier.
    /// </summary>
    /// <param name="claimId">The claim identifier.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteAsync(int claimId);
}