namespace learning_center_webapi.Contexts.Reminders.Domain.Exceptions;

/// <summary>
/// Exception thrown when date or time format is invalid.
/// </summary>
public sealed class InvalidReminderDateFormatException : ReminderBusinessRuleException
{
    public string ProvidedDate { get; }
    public string ExpectedFormat { get; }

    public InvalidReminderDateFormatException(string date) 
        : base($"Invalid date format '{date}'. Expected format: yyyy-MM-dd")
    {
        ProvidedDate = date;
        ExpectedFormat = "yyyy-MM-dd";
    }
}

public sealed class InvalidReminderTimeFormatException : ReminderBusinessRuleException
{
    public string ProvidedTime { get; }
    public string ExpectedFormat { get; }

    public InvalidReminderTimeFormatException(string time) 
        : base($"Invalid time format '{time}'. Expected format: HH:mm")
    {
        ProvidedTime = time;
        ExpectedFormat = "HH:mm";
    }
}