
using learning_center_webapi.Contexts.Reminders.Domain.Infraestructure;
using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Reminders.Domain.Queries;

namespace learning_center_webapi.Contexts.Reminders.Application.QueryServices;

public class ReminderQueryService
{
    private readonly IReminderRepository _reminderRepository;

    public ReminderQueryService(IReminderRepository reminderRepository)
    {
        _reminderRepository = reminderRepository;
    }

    public async Task<IEnumerable<Reminder>> Handle(GetAllRemindersQuery query)
    {
        return await _reminderRepository.GetAllAsync();
    }

    public async Task<Reminder?> Handle(GetReminderByIdQuery query)
    {
        return await _reminderRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Reminder>> Handle(GetRemindersByUserIdQuery query)
    {
        return await _reminderRepository.GetByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Reminder>> Handle(GetUpcomingRemindersQuery query)
    {
        var reminders = await _reminderRepository.GetByUserIdAsync(query.UserId);
        return reminders.Where(r => r.IsUpcoming()).OrderBy(r => r.Date.Value);
    }
}