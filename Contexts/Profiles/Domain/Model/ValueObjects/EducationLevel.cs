
namespace learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;

public record EducationLevel
{
    public string Value { get; }

    private static readonly HashSet<string> ValidLevels = new()
    {
        "Preuniversitario", "Universitario", "Postgrado"
    };

    public EducationLevel(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Education level cannot be empty");

        value = value.Trim();

        if (!ValidLevels.Contains(value))
            throw new ArgumentException($"Invalid education level: {value}. Valid values: {string.Join(", ", ValidLevels)}");

        Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string(EducationLevel level) => level.Value;
}