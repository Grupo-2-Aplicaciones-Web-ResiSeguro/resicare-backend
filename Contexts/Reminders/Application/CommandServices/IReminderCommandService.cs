using learning_center_webapi.Contexts.Reminders.Domain.Commands;
using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;

namespace learning_center_webapi.Contexts.Reminders.Application.CommandServices;

public interface IReminderCommandService
{
    /// <summary>
    /// creation of a new reminder
    /// </summary>
    /// <param name="command">Command containing reminder creation data</param>
    /// <returns>The created reminder entity</returns>
    Task<Reminder> Handle(CreateReminderCommand command);

    /// <summary>
    /// update of a exixting reminder
    /// </summary>
    /// <param name="command">command containing reminder update data</param>
    /// <returns>The updated reminder entity</returns>
    Task<Reminder> Handle(UpdateReminderCommand command);

    /// <summary>
    /// deletion of a reminder
    /// </summary>
    /// <param name="command">command containing reminder identifier</param>
    /// <returns>delete</returns>
    Task<bool> Handle(DeleteReminderCommand command);
}