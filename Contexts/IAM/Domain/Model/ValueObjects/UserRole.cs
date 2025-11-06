namespace learning_center_webapi.Contexts.IAM.Domain.Model.ValueObjects;

public record UserRole
{
    public string Value { get; }

    private static readonly HashSet<string> ValidRoles = new()
    {
        "cliente", "adviser", "asesor", "admin"
    };

    public UserRole(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            value = "cliente"; 

        value = value.ToLowerInvariant().Trim();

        if (!ValidRoles.Contains(value))
            throw new ArgumentException($"Invalid role: {value}. Valid roles: {string.Join(", ", ValidRoles)}");

        Value = value;
    }

    public bool IsAdviser() => Value == "adviser" || Value == "asesor";
    public bool IsClient() => Value == "cliente";
    public bool IsAdmin() => Value == "admin";

    public override string ToString() => Value;

    public static implicit operator string(UserRole role) => role.Value;
}