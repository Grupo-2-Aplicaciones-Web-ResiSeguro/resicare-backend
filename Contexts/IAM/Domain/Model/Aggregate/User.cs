using learning_center_webapi.Contexts.IAM.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Shared.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.IAM.Domain.Model.Aggregates;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Email Email { get; private set; } = default!;
    public PasswordHash PasswordHash { get; private set; } = default!;
    public UserRole Role { get; set; }

    public User() { }

    public User(string name, Email email, PasswordHash passwordHash, UserRole role)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public bool VerifyPassword(string plainPassword)
    {
        return PasswordHash.Verify(plainPassword);
    }

    public void UpdatePassword(PasswordHash newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }
}