namespace learning_center_webapi.Contexts.Reminders.Domain.Exceptions;
public sealed class ReminderInPastException : ReminderBusinessRuleException
{
    public DateTime AttemptedDateTime { get; }

    public ReminderInPastException(DateTime attemptedDateTime) 
        : base("Cannot create reminders in the past")
    {
        AttemptedDateTime = attemptedDateTime;
    }
}