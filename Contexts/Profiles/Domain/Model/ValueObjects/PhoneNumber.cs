using System.Text.RegularExpressions;

namespace learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;

public record PhoneNumber
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        value = value?.Trim() ?? string.Empty;

        if (!string.IsNullOrEmpty(value) && !IsValidPhone(value))
            throw new ArgumentException($"Invalid phone format: {value}");

        Value = value;
    }

    private static bool IsValidPhone(string phone)
    {
        if (phone.Length < 4 || phone.Length > 20)
            return false;

        var regex = new Regex(@"^[0-9+()\s-]+$");
        return regex.IsMatch(phone);
    }

    public static PhoneNumber Empty() => new PhoneNumber(string.Empty);

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phone) => phone.Value;
}