using System.Text.RegularExpressions;

namespace learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;

public record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty");

        value = value.Trim().ToLowerInvariant();

        if (!IsValidEmail(value))
            throw new ArgumentException($"Invalid email format: {value}");

        Value = value;
    }

    private static bool IsValidEmail(string email)
    {
        var regex = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w+$");
        return regex.IsMatch(email);
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}