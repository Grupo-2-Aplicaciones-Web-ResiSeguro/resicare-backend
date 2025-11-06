namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Resources;

public record UpdateRegisteredObjectResource(
    string? Tipo,
    string? Nombre,
    string? DescripcionBreve,
    decimal? Precio,
    string? NumeroSerie,
    string? Foto,
    DateTime? FechaRegistro
);
