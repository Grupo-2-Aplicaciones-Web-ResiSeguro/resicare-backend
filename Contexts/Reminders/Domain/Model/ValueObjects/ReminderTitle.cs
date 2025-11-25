using learning_center_webapi.Contexts.Reminders.Domain.Exceptions;

namespace learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;

public record ReminderTitle
{
    public string Value { get; }
    private const int MinLength = 3;
    private const int MaxLength = 80;

    public ReminderTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidReminderTitleException(value ?? string.Empty, MinLength, MaxLength);

        value = value.Trim();

        if (value.Length < MinLength || value.Length > MaxLength)
            throw new InvalidReminderTitleException(value, MinLength, MaxLength);

        Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string(ReminderTitle title) => title.Value;
}