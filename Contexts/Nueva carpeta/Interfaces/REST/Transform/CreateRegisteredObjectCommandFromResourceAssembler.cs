using learning_center_webapi.Contexts.RegisteredObjects.Domain.Commands;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Transform;

public static class CreateRegisteredObjectCommandFromResourceAssembler
{
    public static CreateRegisteredObjectCommand ToCommand(CreateRegisteredObjectResource resource)
    {
        var registrationDate = resource.FechaRegistro ?? DateTime.UtcNow;
        return new CreateRegisteredObjectCommand(
            resource.Tipo,
            resource.Nombre,
            resource.DescripcionBreve,
            resource.Precio,
            resource.NumeroSerie,
            resource.Foto,
            registrationDate,
            resource.UserId
        );
    }
}
