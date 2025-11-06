namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Commands;

public record CreateRegisteredObjectCommand(
    string Tipo,
    string Nombre,
    string DescripcionBreve,
    decimal Precio,
    string NumeroSerie,
    string Foto,
    DateTime FechaRegistro,
    int UserId
);
