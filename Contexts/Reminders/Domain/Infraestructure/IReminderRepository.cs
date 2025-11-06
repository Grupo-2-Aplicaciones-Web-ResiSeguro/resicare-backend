
using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Reminders.Domain.Infraestructure;

public interface IReminderRepository : IBaseRepository<Reminder>
{
    Task<IEnumerable<Reminder>> GetAllAsync();
    Task<IEnumerable<Reminder>> GetByUserIdAsync(int userId);
}