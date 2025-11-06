namespace learning_center_webapi.Contexts.IAM.Domain.Commands;

public record SignUpCommand
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public string? Rol { get; init; }
}