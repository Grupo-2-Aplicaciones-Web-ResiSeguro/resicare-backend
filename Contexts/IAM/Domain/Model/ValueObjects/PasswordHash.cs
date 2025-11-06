using System.Security.Cryptography;
using System.Text;

namespace learning_center_webapi.Contexts.IAM.Domain.Model.ValueObjects;

public record PasswordHash
{
    public string Value { get; }

    private PasswordHash(string hashedValue)
    {
        Value = hashedValue;
    }

    public static PasswordHash FromPlainPassword(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new ArgumentException("Password cannot be empty");

        if (plainPassword.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters");

        var hash = ComputeHash(plainPassword);
        return new PasswordHash(hash);
    }

    public static PasswordHash FromHash(string hashedValue)
    {
        if (string.IsNullOrWhiteSpace(hashedValue))
            throw new ArgumentException("Hash cannot be empty");

        return new PasswordHash(hashedValue);
    }

    public bool Verify(string plainPassword)
    {
        var inputHash = ComputeHash(plainPassword);
        return inputHash == Value;
    }

    private static string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public override string ToString() => Value;
}