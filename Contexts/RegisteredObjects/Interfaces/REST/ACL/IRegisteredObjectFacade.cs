namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.ACL;

public interface IRegisteredObjectFacade
{
    /// <summary>
    ///  Validates if a registered object ID exists.
    /// </summary>
    Task<bool> IsValidRegisteredObjectId(int registeredObjectId);
}
