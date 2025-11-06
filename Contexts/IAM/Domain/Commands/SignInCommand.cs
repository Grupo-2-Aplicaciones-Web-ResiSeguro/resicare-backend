namespace learning_center_webapi.Contexts.IAM.Domain.Commands;

public record SignInCommand
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}