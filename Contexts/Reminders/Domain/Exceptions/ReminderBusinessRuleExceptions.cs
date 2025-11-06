
namespace learning_center_webapi.Contexts.Reminders.Domain.Exceptions;

public abstract class ReminderBusinessRuleException : Exception
{
    protected ReminderBusinessRuleException(string message) : base(message) { }
}

public class ReminderNotFoundException : ReminderBusinessRuleException
{
    public ReminderNotFoundException(int id) 
        : base($"Reminder with id {id} not found") { }
}

public class ReminderInPastException : ReminderBusinessRuleException
{
    public ReminderInPastException() 
        : base("Cannot create reminders in the past") { }
}

public class InvalidReminderDataException : ReminderBusinessRuleException
{
    public InvalidReminderDataException(string message) 
        : base(message) { }
}