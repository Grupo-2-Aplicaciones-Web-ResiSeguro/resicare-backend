using System.Globalization;
using learning_center_webapi.Contexts.Reminders.Domain.Exceptions;

namespace learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;

public record ReminderDate
{
    public string Value { get; }

    public ReminderDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidReminderDateFormatException(value ?? string.Empty);

        value = value.Trim();

        if (!DateTime.TryParseExact(value, "yyyy-MM-dd", 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            throw new InvalidReminderDateFormatException(value);

        Value = value;
    }

    public DateTime ToDateTime()
    {
        return DateTime.ParseExact(Value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public override string ToString() => Value;

    public static implicit operator string(ReminderDate date) => date.Value;
}