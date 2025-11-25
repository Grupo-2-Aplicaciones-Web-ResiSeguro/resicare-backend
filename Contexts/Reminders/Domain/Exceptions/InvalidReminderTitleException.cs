namespace learning_center_webapi.Contexts.Reminders.Domain.Exceptions;
public sealed class InvalidReminderTitleException : ReminderBusinessRuleException
{
    public string ProvidedTitle { get; }
    public int MinLength { get; }
    public int MaxLength { get; }

    public InvalidReminderTitleException(string title, int minLength = 3, int maxLength = 80) 
        : base($"Reminder title must be between {minLength} and {maxLength} characters")
    {
        ProvidedTitle = title;
        MinLength = minLength;
        MaxLength = maxLength;
    }
}