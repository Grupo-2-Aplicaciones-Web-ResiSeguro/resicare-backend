// Contexts/Teleconsultations/Infraestructure/TeleconsultationRepository.cs
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Infraestructure;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.Teleconsultations.Infraestructure;

/// <summary>
/// Repository implementation for teleconsultation data access.
/// </summary>
public class TeleconsultationRepository : BaseRepository<Teleconsultation>, ITeleconsultationRepository
{
    private readonly LearningCenterContext _context;

    public TeleconsultationRepository(LearningCenterContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all teleconsultations.
    /// </summary>
    /// <returns>A collection of all teleconsultations.</returns>
    public async Task<IEnumerable<Teleconsultation>> GetAllAsync()
    {
        return await _context.Set<Teleconsultation>().ToListAsync();
    }

    /// <summary>
    /// Retrieves teleconsultations by user identifier.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of teleconsultations for the specified user.</returns>
    public async Task<IEnumerable<Teleconsultation>> GetByUserIdAsync(int userId)
    {
        return await _context.Set<Teleconsultation>()
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves teleconsultations by service type.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <returns>A collection of teleconsultations for the specified service.</returns>
    public async Task<IEnumerable<Teleconsultation>> GetByServiceAsync(string service)
    {
        return await _context.Set<Teleconsultation>()
            .Where(t => t.Service == service)
            .ToListAsync();
    }

    /// <summary>
    /// Finds a teleconsultation for a specific user at a specific date and time.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="date">The appointment date.</param>
    /// <param name="time">The appointment time.</param>
    /// <returns>The teleconsultation if found, otherwise null.</returns>
    public async Task<Teleconsultation?> FindByUserDateAndTimeAsync(int userId, string date, string time)
    {
        return await _context.Set<Teleconsultation>()
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Date == date && t.Time == time);
    }

    /// <summary>
    /// Counts teleconsultations for a specific service at a specific date and time.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="date">The appointment date.</param>
    /// <param name="time">The appointment time.</param>
    /// <returns>The number of teleconsultations for that service slot.</returns>
    public async Task<int> CountByServiceDateAndTimeAsync(string service, string date, string time)
    {
        return await _context.Set<Teleconsultation>()
            .Where(t => t.Service == service && t.Date == date && t.Time == time)
            .CountAsync();
    }

    /// <summary>
    /// Counts active teleconsultations for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The number of active teleconsultations.</returns>
    public async Task<int> CountActiveTeleconsultationsByUserAsync(int userId)
    {
        var now = DateTime.UtcNow;
        var today = now.ToString("yyyy-MM-dd");

        return await _context.Set<Teleconsultation>()
            .Where(t => t.UserId == userId)
            .Where(t => string.Compare(t.Date, today) >= 0) // Future or today
            .CountAsync();
    }
}