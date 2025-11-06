using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Repositories;

public interface IRegisteredObjectRepository : IBaseRepository<RegisteredObject>
{
    /// <summary>
    /// Retrieves objects filtered by user identifier.
    /// </summary>
    Task<IEnumerable<RegisteredObject>> FindByUserIdAsync(int userId);

    /// <summary>
    /// Performs a text search across name and description fields.
    /// </summary>
    Task<IEnumerable<RegisteredObject>> SearchAsync(string? search, int? userId);
}
