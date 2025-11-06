
namespace learning_center_webapi.Contexts.Profiles.Domain.Model.ValueObjects;

public record Address
{
    public string Value { get; }

    public Address(string value)
    {
        value = value?.Trim() ?? string.Empty;

        if (value.Length > 200)
            throw new ArgumentException("Address cannot exceed 200 characters");

        Value = value;
    }

    public static Address Empty() => new Address(string.Empty);

    public override string ToString() => Value;

    public static implicit operator string(Address address) => address.Value;
}