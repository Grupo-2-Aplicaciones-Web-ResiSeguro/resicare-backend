// Contexts/Claims/Infraestructure/ClaimRepository.cs
using System.Linq;
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Claims.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.Claims.Infraestructure;

/// <summary>
/// Repository implementation for claim data access.
/// </summary>
public class ClaimRepository(LearningCenterContext context) : BaseRepository<Claim>(context), IClaimRepository
{
    private readonly LearningCenterContext _context = context;

    /// <summary>
    /// Retrieves claims filtered by user identifier.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of claims belonging to the user.</returns>
    public async Task<IEnumerable<Claim>> FindByUserIdAsync(int userId)
    {
        return await _context.Set<Claim>()
            .Include(c => c.Documents)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves claims filtered by status.
    /// </summary>
    /// <param name="status">The claim status.</param>
    /// <returns>A collection of claims with the specified status.</returns>
    public async Task<IEnumerable<Claim>> FindByStatusAsync(string status)
    {
        return await _context.Set<Claim>()
            .Include(c => c.Documents)
            .Where(c => c.Status == status)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves claims with optional status and user filters.
    /// </summary>
    /// <param name="status">Optional status filter.</param>
    /// <param name="userId">Optional user identifier filter.</param>
    /// <returns>A collection of filtered claims.</returns>
    public async Task<IEnumerable<Claim>> SearchAsync(string? status, int? userId)
    {
        var query = _context.Set<Claim>().Include(c => c.Documents).AsQueryable();
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(c => c.Status == status);
        if (userId.HasValue) query = query.Where(c => c.UserId == userId);
        return await query.OrderByDescending(c => c.CreatedDate).ToListAsync();
    }

    /// <summary>
    /// Retrieves a claim including related documents.
    /// </summary>
    /// <param name="claimId">The claim identifier.</param>
    /// <returns>The claim with documents, or null if not found.</returns>
    public async Task<Claim?> FindWithDocumentsByIdAsync(int claimId)
    {
        return await _context.Set<Claim>()
            .Include(c => c.Documents)
            .FirstOrDefaultAsync(c => c.Id == claimId);
    }

    /// <summary>
    /// Finds an active claim (pending or in_review) for a registered object.
    /// </summary>
    /// <param name="registeredObjectId">The registered object identifier.</param>
    /// <returns>The active claim if found, otherwise null.</returns>
    public async Task<Claim?> FindActiveClaimByRegisteredObjectIdAsync(int registeredObjectId)
    {
        return await _context.Set<Claim>()
            .Where(c => c.RegisteredObjectId == registeredObjectId)
            .Where(c => c.Status == "pending" || c.Status == "in_review")
            .FirstOrDefaultAsync();
    }
}