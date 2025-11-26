// Contexts/RegisteredObjects/Application/QueryServices/IRegisteredObjectQueryService.cs
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Queries;

namespace learning_center_webapi.Contexts.RegisteredObjects.Application.QueryServices;

/// <summary>
/// Interface for registered object query operations
/// </summary>
public interface IRegisteredObjectQueryService
{
    Task<IEnumerable<RegisteredObject>> Handle(GetAllRegisteredObjectsQuery query);
    Task<IEnumerable<RegisteredObject>> Handle(GetRegisteredObjectsByUserIdQuery query);
    Task<RegisteredObject?> Handle(GetRegisteredObjectByIdQuery query);
}