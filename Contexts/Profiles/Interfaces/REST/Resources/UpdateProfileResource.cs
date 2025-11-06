namespace learning_center_webapi.Contexts.Profiles.Interfaces.REST.Resources;

public record UpdateProfileResource
{
    public string? Nombre { get; init; }
    public string? Correo { get; init; }
    public int? Edad { get; init; }
    public string? Residencia { get; init; }
    public string? Telefono { get; init; }
    public string? Genero { get; init; }
    public string? NivelInstruccion { get; init; }
    public string? PhotoDni { get; init; }
    public string? PhotoCredential { get; init; }
    public string? Bio { get; init; }
}