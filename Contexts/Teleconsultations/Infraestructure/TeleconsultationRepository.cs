
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Infraestructure;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.Teleconsultations.Infraestructure;

public class TeleconsultationRepository : BaseRepository<Teleconsultation>, ITeleconsultationRepository
{
    private readonly LearningCenterContext _context;

    public TeleconsultationRepository(LearningCenterContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<IEnumerable<Teleconsultation>> GetAllAsync()
    {
        return await _context.Set<Teleconsultation>().ToListAsync();
    }

    public async Task<IEnumerable<Teleconsultation>> GetByUserIdAsync(int userId) 
    {
        return await _context.Set<Teleconsultation>()
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Teleconsultation>> GetByServiceAsync(string service)
    {
        return await _context.Set<Teleconsultation>()
            .Where(t => t.Service == service)
            .ToListAsync();
    }
}