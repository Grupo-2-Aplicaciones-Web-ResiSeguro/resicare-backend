
namespace learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;

public record Age
{
    public int Value { get; }

    public Age(int value)
    {
        if (value < 0)
            throw new ArgumentException("Age cannot be negative");

        if (value > 120)
            throw new ArgumentException("Age cannot exceed 120");

        Value = value;
    }

    public static Age Default() => new Age(0);

    public override string ToString() => Value.ToString();

    public static implicit operator int(Age age) => age.Value;
}