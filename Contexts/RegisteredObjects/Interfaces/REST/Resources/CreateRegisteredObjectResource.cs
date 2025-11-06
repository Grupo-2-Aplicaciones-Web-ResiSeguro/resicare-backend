namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Resources;

public record CreateRegisteredObjectResource(
    string Tipo,
    string Nombre,
    string DescripcionBreve,
    decimal Precio,
    string NumeroSerie,
    string Foto,
    DateTime? FechaRegistro,
    int UserId
);
