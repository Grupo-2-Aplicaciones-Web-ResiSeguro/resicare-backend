// Contexts/Claims/Domain/Repositories/IClaimRepository.cs
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Claims.Domain.Repositories;

public interface IClaimRepository : IBaseRepository<Claim>
{
    /// <summary>
    /// Retrieves claims filtered by user identifier.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of claims belonging to the user.</returns>
    Task<IEnumerable<Claim>> FindByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves claims filtered by status.
    /// </summary>
    /// <param name="status">The claim status.</param>
    /// <returns>A collection of claims with the specified status.</returns>
    Task<IEnumerable<Claim>> FindByStatusAsync(string status);

    /// <summary>
    /// Retrieves claims with optional status and user filters.
    /// </summary>
    /// <param name="status">Optional status filter.</param>
    /// <param name="userId">Optional user identifier filter.</param>
    /// <returns>A collection of filtered claims.</returns>
    Task<IEnumerable<Claim>> SearchAsync(string? status, int? userId);

    /// <summary>
    /// Retrieves a claim including related documents.
    /// </summary>
    /// <param name="claimId">The claim identifier.</param>
    /// <returns>The claim with documents, or null if not found.</returns>
    Task<Claim?> FindWithDocumentsByIdAsync(int claimId);

    /// <summary>
    /// Finds an active claim (pending or in_review) for a registered object.
    /// </summary>
    /// <param name="registeredObjectId">The registered object identifier.</param>
    /// <returns>The active claim if found, otherwise null.</returns>
    Task<Claim?> FindActiveClaimByRegisteredObjectIdAsync(int registeredObjectId);
}