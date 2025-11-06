
using learning_center_webapi.Contexts.Profiles.Domain.Infraestructure;
using learning_center_webapi.Contexts.Profiles.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.Profiles.Infraestructure;

public class ProfileRepository : BaseRepository<Profile>, IProfileRepository
{
    private readonly LearningCenterContext _context;

    public ProfileRepository(LearningCenterContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Profile>> GetAllAsync()
    {
        return await _context.Set<Profile>().ToListAsync();
    }

    public async Task<Profile?> FindByUserIdAsync(int userId)
    {
        return await _context.Set<Profile>()
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }
}