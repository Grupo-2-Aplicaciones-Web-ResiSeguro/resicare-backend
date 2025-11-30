using learning_center_webapi.Contexts.RegisteredObjects.Application.QueryServices;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Queries;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.ACL;

namespace learning_center_webapi.Contexts.RegisteredObjects.Application.ACL;

public class RegisteredObjectFacade(RegisteredObjectQueryService registeredObjectQueryService) : IRegisteredObjectFacade
{
    public async Task<bool> IsValidRegisteredObjectId(int registeredObjectId)
    {
        var query = new GetRegisteredObjectByIdQuery(registeredObjectId);
        var existingObject = await registeredObjectQueryService.Handle(query);

        var isValid = (existingObject != null) ? true : false;
        return isValid;
    }
}
