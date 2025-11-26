using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Repositories;

public interface IRegisteredObjectRepository : IBaseRepository<RegisteredObject>
{
    /// <summary>
    /// retrieves objects filtered by user identifier
    /// </summary>
    /// <param name="userId">the user identifier</param>
    /// <returns>collection of objects belonging to the user</returns>
    Task<IEnumerable<RegisteredObject>> FindByUserIdAsync(int userId);

    /// <summary>
    /// Performs a text search across name and description fields.
    /// </summary>
    /// <param name="search">search text</param>
    /// <param name="userId">optional user identifier filter</param>
    /// <returns>collection of matching registered objects</returns>
    Task<IEnumerable<RegisteredObject>> SearchAsync(string? search, int? userId);

    /// <summary>
    /// finds serial number for a user
    /// </summary>
    /// <param name="serialNumber">the serial number to search.</param>
    /// <param name="userId">ser identifier</param>
    /// <returns>registered object if found</returns>
    Task<RegisteredObject?> FindBySerialNumberAndUserAsync(string serialNumber, int userId);
}