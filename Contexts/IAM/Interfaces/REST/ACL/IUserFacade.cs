namespace learning_center_webapi.Contexts.IAM.Interfaces.REST.ACL;

public interface IUserFacade
{
    /// <summary>
    ///  Validates if a user ID exists.
    /// </summary>
    Task<bool> IsValidUserId(int userId);
}
