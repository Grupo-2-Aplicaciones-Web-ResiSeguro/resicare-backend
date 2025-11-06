using learning_center_webapi.Contexts.Reminders.Domain.Commands;
using learning_center_webapi.Contexts.Reminders.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Reminders.Interfaces.REST.Transform;

public static class CreateReminderCommandFromResourceAssembler
{
    public static CreateReminderCommand ToCommand(CreateReminderResource resource)
    {
        return new CreateReminderCommand
        {
            UserId = resource.UserId,
            Title = resource.Title,
            Type = resource.Type,
            Date = resource.Date,
            Time = resource.Time,
            Notes = resource.Notes
        };
    }
}