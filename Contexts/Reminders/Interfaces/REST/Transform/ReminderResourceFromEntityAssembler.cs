using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Reminders.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Reminders.Interfaces.REST.Transform;

public static class ReminderResourceFromEntityAssembler
{
    public static ReminderResource ToResource(Reminder reminder)
    {
        return new ReminderResource
        {
            Id = reminder.Id,
            UserId = reminder.UserId,
            Title = reminder.Title.Value,
            Type = reminder.Type.Value,
            Date = reminder.Date.Value,
            Time = reminder.Time.Value,
            Notes = reminder.Notes,
            CreatedAt = reminder.CreatedDate.ToString("o"),
            UpdatedAt = reminder.UpdatedDate?.ToString("o") ?? reminder.CreatedDate.ToString("o")
        };
    }
}