namespace learning_center_webapi.Contexts.Profiles.Interfaces.REST.Resources;

public record ProfileResource
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Correo { get; init; } = string.Empty;
    public int Edad { get; init; }
    public string Residencia { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string Genero { get; init; } = string.Empty;
    public string NivelInstruccion { get; init; } = string.Empty;
    public string? PhotoDni { get; init; }
    public string? PhotoCredential { get; init; }
    public string? Bio { get; init; }
    public string CreatedAt { get; init; } = string.Empty;
}