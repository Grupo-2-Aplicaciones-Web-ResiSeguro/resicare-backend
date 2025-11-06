namespace learning_center_webapi.Contexts.Profiles.Interfaces.REST.Resources;

public record CreateProfileResource
{
    public required int UserId { get; init; }
    public required string Nombre { get; init; }
    public required string Correo { get; init; }
    public int? Edad { get; init; }
    public string? Residencia { get; init; }
    public string? Telefono { get; init; }
    public required string Genero { get; init; }
    public required string NivelInstruccion { get; init; }
    public string? PhotoDni { get; init; }
    public string? PhotoCredential { get; init; }
    public string? Bio { get; init; }
}