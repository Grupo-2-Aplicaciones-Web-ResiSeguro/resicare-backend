using System.Linq;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.RegisteredObjects.Infraestructure;

public class RegisteredObjectRepository(LearningCenterContext context) : BaseRepository<RegisteredObject>(context), IRegisteredObjectRepository
{
    private readonly LearningCenterContext _context = context;

    public async Task<IEnumerable<RegisteredObject>> FindByUserIdAsync(int userId)
    {
        return await _context.Set<RegisteredObject>()
            .Where(obj => obj.UserId == userId)
            .OrderByDescending(obj => obj.FechaRegistro)
            .ToListAsync();
    }

    public async Task<IEnumerable<RegisteredObject>> SearchAsync(string? search, int? userId)
    {
        var query = _context.Set<RegisteredObject>().AsQueryable();
        if (userId.HasValue) query = query.Where(obj => obj.UserId == userId);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search.Trim()}%";
            query = query.Where(obj =>
                EF.Functions.Like(obj.Nombre, pattern) ||
                EF.Functions.Like(obj.DescripcionBreve, pattern));
        }
        return await query.OrderByDescending(obj => obj.FechaRegistro).ToListAsync();
    }
}
