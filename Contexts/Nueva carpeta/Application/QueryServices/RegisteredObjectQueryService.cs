using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Queries;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Repositories;

namespace learning_center_webapi.Contexts.RegisteredObjects.Application.QueryServices;

public class RegisteredObjectQueryService(IRegisteredObjectRepository repository) : IRegisteredObjectQueryService
{
    public async Task<IEnumerable<RegisteredObject>> Handle(GetAllRegisteredObjectsQuery query)
    {
        return await repository.SearchAsync(query.Search, query.UserId);
    }

    public async Task<IEnumerable<RegisteredObject>> Handle(GetRegisteredObjectsByUserIdQuery query)
    {
        return await repository.FindByUserIdAsync(query.UserId);
    }

    public async Task<RegisteredObject?> Handle(GetRegisteredObjectByIdQuery query)
    {
        return await repository.FindByIdAsync(query.ObjectId);
    }
}
