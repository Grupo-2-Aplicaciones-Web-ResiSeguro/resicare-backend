using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Reminders.Domain.Queries;

namespace learning_center_webapi.Contexts.Reminders.Application.QueryServices;

public interface IReminderQueryService
{
    /// <summary>
    /// retrieves all reminders
    /// </summary>
    /// <param name="query">query for reminders</param>
    /// <returns>collection of reminders</returns>
    Task<IEnumerable<Reminder>> Handle(GetAllRemindersQuery query);

    /// <summary>
    /// reminder its identifier
    /// </summary>
    /// <param name="query">1uery containing identifier</param>
    /// <returns>The reminder if found, otherwise null.</returns>
    Task<Reminder?> Handle(GetReminderByIdQuery query);

    /// <summary>
    /// retrieves reminders by user identifier.
    /// </summary>
    /// <param name="query">query containing user identifier.</param>
    /// <returns>A collection of reminders for the specified user.</returns>
    Task<IEnumerable<Reminder>> Handle(GetRemindersByUserIdQuery query);

    /// <summary>
    /// Retrieves upcoming reminders for a user
    /// </summary>
    /// <param name="query">query containing user identifier</param>
    /// <returns>A collection of upcoming reminders</returns>
    Task<IEnumerable<Reminder>> Handle(GetUpcomingRemindersQuery query);
}