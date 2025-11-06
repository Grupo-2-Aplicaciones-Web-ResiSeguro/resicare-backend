
using learning_center_webapi.Contexts.IAM.Domain.Infraestructure;
using learning_center_webapi.Contexts.IAM.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.IAM.Infraestructure;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly LearningCenterContext _context;

    public UserRepository(LearningCenterContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Set<User>().ToListAsync();
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        
        var allUsers = await _context.Set<User>().ToListAsync();
        return allUsers.FirstOrDefault(u => 
            u.Email.Value.ToLowerInvariant() == normalizedEmail);
    }
}