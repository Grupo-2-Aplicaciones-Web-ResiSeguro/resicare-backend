using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
}