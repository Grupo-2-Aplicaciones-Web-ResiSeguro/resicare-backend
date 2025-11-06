using System.Linq;
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Claims.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.Claims.Infraestructure;

public class ClaimRepository(LearningCenterContext context) : BaseRepository<Claim>(context), IClaimRepository
{
    private readonly LearningCenterContext _context = context;

    public async Task<IEnumerable<Claim>> FindByUserIdAsync(int userId)
    {
        return await _context.Set<Claim>()
            .Include(c => c.Documents)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Claim>> FindByStatusAsync(string status)
    {
        return await _context.Set<Claim>()
            .Include(c => c.Documents)
            .Where(c => c.Status == status)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Claim>> SearchAsync(string? status, int? userId)
    {
        var query = _context.Set<Claim>().Include(c => c.Documents).AsQueryable();
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(c => c.Status == status);
        if (userId.HasValue) query = query.Where(c => c.UserId == userId);
        return await query.OrderByDescending(c => c.CreatedDate).ToListAsync();
    }

    public async Task<Claim?> FindWithDocumentsByIdAsync(int claimId)
    {
        return await _context.Set<Claim>()
            .Include(c => c.Documents)
            .FirstOrDefaultAsync(c => c.Id == claimId);
    }
}
