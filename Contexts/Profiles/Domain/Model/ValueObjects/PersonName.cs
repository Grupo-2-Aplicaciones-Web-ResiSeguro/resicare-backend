
namespace learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;

public record PersonName
{
    public string Value { get; }

    public PersonName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be empty");

        value = value.Trim();

        if (value.Length < 3)
            throw new ArgumentException("Name must be at least 3 characters");

        if (value.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters");

        Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string(PersonName name) => name.Value;
}