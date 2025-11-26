using learning_center_webapi.Contexts.RegisteredObjects.Domain.Commands;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.RegisteredObjects.Application.CommandServices;

/// <summary>
/// registered object command operations
/// </summary>
public interface IRegisteredObjectCommandService
{
    /// <summary>
    /// Handles the creation of a new registered object
    /// </summary>
    /// <param name="command">command containing creation data</param>
    /// <returns>the created registered object entity</returns>
    Task<RegisteredObject> Handle(CreateRegisteredObjectCommand command);

    /// <summary>
    /// Handles the update of an existing registered object
    /// </summary>
    /// <param name="command">command containing update data</param>
    /// <returns>the updated registered entity</returns>
    Task<RegisteredObject?> Handle(UpdateRegisteredObjectCommand command);

    /// <summary>
    /// Deletes a registered object by its identifier
    /// </summary>
    /// <param name="objectId">the object identifier</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> DeleteAsync(int objectId);
}