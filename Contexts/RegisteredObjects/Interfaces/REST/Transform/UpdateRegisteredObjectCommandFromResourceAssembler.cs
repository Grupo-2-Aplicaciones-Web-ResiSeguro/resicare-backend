using learning_center_webapi.Contexts.RegisteredObjects.Domain.Commands;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Transform;

public static class UpdateRegisteredObjectCommandFromResourceAssembler
{
    public static UpdateRegisteredObjectCommand ToCommand(int objectId, UpdateRegisteredObjectResource resource)
    {
        return new UpdateRegisteredObjectCommand(
            objectId,
            resource.Tipo,
            resource.Nombre,
            resource.DescripcionBreve,
            resource.Precio,
            resource.NumeroSerie,
            resource.Foto,
            resource.FechaRegistro
        );
    }
}
