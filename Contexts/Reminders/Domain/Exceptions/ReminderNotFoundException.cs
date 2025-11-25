namespace learning_center_webapi.Contexts.Reminders.Domain.Exceptions;
public sealed class ReminderNotFoundException : ReminderBusinessRuleException
{
    public int ReminderId { get; }

    public ReminderNotFoundException(int id) 
        : base($"Reminder with id {id} not found")
    {
        ReminderId = id;
    }
}