using learning_center_webapi.Contexts.Reminders.Domain.Commands;
using learning_center_webapi.Contexts.Reminders.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Reminders.Interfaces.REST.Transform;

public static class UpdateReminderCommandFromResourceAssembler
{
    public static UpdateReminderCommand ToCommand(int id, UpdateReminderResource resource)
    {
        return new UpdateReminderCommand
        {
            Id = id,
            Title = resource.Title,
            Type = resource.Type,
            Date = resource.Date,
            Time = resource.Time,
            Notes = resource.Notes
        };
    }
}