namespace learning_center_webapi.Contexts.IAM.Interfaces.REST.Resources;

public record SignInResource
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}