namespace learning_center_webapi.Contexts.Reminders.Domain.Exceptions;

public abstract class ReminderBusinessRuleException : Exception
{
    protected ReminderBusinessRuleException(string message) : base(message) { }
    protected ReminderBusinessRuleException(string message, Exception inner) : base(message, inner) { }
}