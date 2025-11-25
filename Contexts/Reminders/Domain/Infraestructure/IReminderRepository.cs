using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Reminders.Domain.Infraestructure;
public interface IReminderRepository : IBaseRepository<Reminder>
{
    /// <summary>
    /// retrieves all reminders
    /// </summary>
    /// <returns>collection of all reminders</returns>
    Task<IEnumerable<Reminder>> GetAllAsync();

    /// <summary>
    /// retrieves reminders by user identifier
    /// </summary>
    /// <param name="userId">the user identifier.</param>
    /// <returns>collection of reminders for the specified user</returns>
    Task<IEnumerable<Reminder>> GetByUserIdAsync(int userId);

    /// <summary>
    /// finds a duplicate reminder for a user
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="title">The reminder title.</param>
    /// <param name="date">The reminder date.</param>
    /// <param name="time">The reminder time.</param>
    /// <param name="type">The reminder type.</param>
    /// <returns>The duplicate reminder if found, otherwise null.</returns>
    Task<Reminder?> FindDuplicateAsync(int userId, string title, string date, string time, string type);
}