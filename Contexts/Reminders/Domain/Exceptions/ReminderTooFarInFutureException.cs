namespace learning_center_webapi.Contexts.Reminders.Domain.Exceptions;
public sealed class ReminderTooFarInFutureException : ReminderBusinessRuleException
{
    public DateTime AttemptedDate { get; }
    public int MaxDaysAllowed { get; }

    public ReminderTooFarInFutureException(DateTime attemptedDate, int maxDaysAllowed = 180) 
        : base($"Cannot create reminders more than {maxDaysAllowed} days in the future")
    {
        AttemptedDate = attemptedDate;
        MaxDaysAllowed = maxDaysAllowed;
    }
}