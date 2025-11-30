namespace learning_center_webapi.Contexts.Profiles.Interfaces.REST.ACL;

public interface IProfileFacade
{
    /// <summary>
    ///  Validates if a profile ID exists.
    /// </summary>
    Task<bool> IsValidProfileId(int profileId);

    /// <summary>
    ///  Validates if a profile exists for a given user ID.
    /// </summary>
    Task<bool> IsValidProfileByUserId(int userId);
}
