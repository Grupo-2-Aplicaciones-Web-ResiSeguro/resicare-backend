using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Transform;

public static class RegisteredObjectResourceFromEntityAssembler
{
    public static RegisteredObjectResource ToResource(RegisteredObject registeredObject)
    {
        return new RegisteredObjectResource(
            registeredObject.Id,
            registeredObject.Tipo,
            registeredObject.Nombre,
            registeredObject.DescripcionBreve,
            registeredObject.Precio,
            registeredObject.NumeroSerie,
            registeredObject.Foto,
            registeredObject.FechaRegistro,
            registeredObject.UserId,
            registeredObject.CreatedDate,
            registeredObject.UpdatedDate
        );
    }
}
