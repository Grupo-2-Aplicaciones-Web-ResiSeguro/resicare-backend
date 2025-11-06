

namespace learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;

public record Gender
{
    public string Value { get; }

    private static readonly HashSet<string> ValidGenders = new()
    {
        "Masculino", "Femenino", "Otro"
    };

    public Gender(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Gender cannot be empty");

        value = value.Trim();

        if (!ValidGenders.Contains(value))
            throw new ArgumentException($"Invalid gender: {value}. Valid values: {string.Join(", ", ValidGenders)}");

        Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string(Gender gender) => gender.Value;
}