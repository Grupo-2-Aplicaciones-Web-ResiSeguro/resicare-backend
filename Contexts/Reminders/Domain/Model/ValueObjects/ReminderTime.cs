using System.Globalization;
using System.Text.RegularExpressions;

namespace learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;

public record ReminderTime
{
    public string Value { get; }

    public ReminderTime(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Time cannot be empty");

        value = value.Trim();

        if (!Regex.IsMatch(value, @"^\d{2}:\d{2}$"))
            throw new ArgumentException($"Invalid time format: {value}. Expected format: HH:mm");

        if (!TimeSpan.TryParseExact(value, @"hh\:mm", 
                CultureInfo.InvariantCulture, out var parsedTime))
            throw new ArgumentException($"Invalid time: {value}");

        Value = value;
    }

    public TimeSpan ToTimeSpan()
    {
        return TimeSpan.ParseExact(Value, @"hh\:mm", CultureInfo.InvariantCulture);
    }

    public override string ToString() => Value;

    public static implicit operator string(ReminderTime time) => time.Value;
}