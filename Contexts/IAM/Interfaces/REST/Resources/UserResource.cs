namespace learning_center_webapi.Contexts.IAM.Interfaces.REST.Resources;

public record UserResource
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Rol { get; init; } = string.Empty;
    public string CreatedAt { get; init; } = string.Empty;
}