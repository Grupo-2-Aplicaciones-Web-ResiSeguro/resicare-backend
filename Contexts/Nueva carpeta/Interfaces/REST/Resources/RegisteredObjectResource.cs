namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Resources;

public record RegisteredObjectResource(
    int Id,
    string Tipo,
    string Nombre,
    string DescripcionBreve,
    decimal Precio,
    string NumeroSerie,
    string Foto,
    DateTime FechaRegistro,
    int UserId,
    DateTime CreatedDate,
    DateTime? UpdatedDate
);
