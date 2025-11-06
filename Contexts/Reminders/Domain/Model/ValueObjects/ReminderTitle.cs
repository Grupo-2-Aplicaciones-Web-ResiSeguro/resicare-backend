using System;

namespace learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;

public record ReminderTitle
{
    public string Value { get; }

    public ReminderTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Reminder title cannot be empty");

        value = value.Trim();

        if (value.Length < 3)
            throw new ArgumentException("Reminder title must be at least 3 characters");

        if (value.Length > 100)
            throw new ArgumentException("Reminder title cannot exceed 100 characters");

        Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string(ReminderTitle title) => title.Value;
}