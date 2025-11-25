using System.Globalization;
using System.Text.RegularExpressions;
using learning_center_webapi.Contexts.Reminders.Domain.Exceptions;

namespace learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;
public record ReminderTime
{
    public string Value { get; }

    public ReminderTime(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidReminderTimeFormatException(value ?? string.Empty);

        value = value.Trim();

        if (!Regex.IsMatch(value, @"^\d{2}:\d{2}$"))
            throw new InvalidReminderTimeFormatException(value);

        if (!TimeSpan.TryParseExact(value, @"hh\:mm", 
                CultureInfo.InvariantCulture, out var parsedTime))
            throw new InvalidReminderTimeFormatException(value);

        Value = value;
    }

    public TimeSpan ToTimeSpan()
    {
        return TimeSpan.ParseExact(Value, @"hh\:mm", CultureInfo.InvariantCulture);
    }

    public override string ToString() => Value;

    public static implicit operator string(ReminderTime time) => time.Value;
}