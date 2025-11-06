using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Claims.Domain.Repositories;

public interface IClaimRepository : IBaseRepository<Claim>
{
    /// <summary>
    /// Retrieves claims filtered by user identifier.
    /// </summary>
    Task<IEnumerable<Claim>> FindByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves claims filtered by status.
    /// </summary>
    Task<IEnumerable<Claim>> FindByStatusAsync(string status);

    /// <summary>
    /// Retrieves claims with optional status and user filters.
    /// </summary>
    Task<IEnumerable<Claim>> SearchAsync(string? status, int? userId);

    /// <summary>
    /// Retrieves a claim including related documents.
    /// </summary>
    Task<Claim?> FindWithDocumentsByIdAsync(int claimId);
}
