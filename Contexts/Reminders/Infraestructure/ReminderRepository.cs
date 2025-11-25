using learning_center_webapi.Contexts.Reminders.Domain.Infraestructure;
using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Shared.Infraestructure.Persistence.Configuration;
using learning_center_webapi.Contexts.Shared.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace learning_center_webapi.Contexts.Reminders.Infraestructure;
public class ReminderRepository : BaseRepository<Reminder>, IReminderRepository
{
    private readonly LearningCenterContext _context;

    public ReminderRepository(LearningCenterContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reminder>> GetAllAsync()
    {
        return await _context.Set<Reminder>().ToListAsync();
    }

    public async Task<IEnumerable<Reminder>> GetByUserIdAsync(int userId)
    {
        var userReminders = await _context.Set<Reminder>()
            .Where(r => r.UserId == userId)
            .ToListAsync();
        
        return userReminders
            .OrderByDescending(r => r.Date.Value)
            .ThenByDescending(r => r.Time.Value)
            .ToList();
    }

    /// <summary>
    /// finds a duplicate reminder for a specific user
    /// </summary>
    /// <param name="userId">user identifier</param>
    /// <param name="title">reminder title</param>
    /// <param name="date">reminder date</param>
    /// <param name="time">eminder time</param>
    /// <param name="type">reminder type</param>
    /// <returns>duplicate reminder if found, otherwise null.</returns>
    public async Task<Reminder?> FindDuplicateAsync(int userId, string title, string date, string time, string type)
    {
        var reminders = await _context.Set<Reminder>()
            .Where(r => r.UserId == userId)
            .ToListAsync();

        return reminders.FirstOrDefault(r => 
            r.Title.Value == title &&
            r.Date.Value == date &&
            r.Time.Value == time &&
            r.Type.Value == type);
    }
}