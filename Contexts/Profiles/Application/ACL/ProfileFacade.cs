using learning_center_webapi.Contexts.Profiles.Application.QueryServices;
using learning_center_webapi.Contexts.Profiles.Domain.Queries;
using learning_center_webapi.Contexts.Profiles.Interfaces.REST.ACL;

namespace learning_center_webapi.Contexts.Profiles.Application.ACL;

public class ProfileFacade(ProfileQueryService profileQueryService) : IProfileFacade
{
    public async Task<bool> IsValidProfileId(int profileId)
    {
        var profileQuery = new GetProfileByIdQuery(profileId);
        var existingProfile = await profileQueryService.Handle(profileQuery);

        var isValid = (existingProfile != null) ? true : false;
        return isValid;
    }

    public async Task<bool> IsValidProfileByUserId(int userId)
    {
        var profileQuery = new GetProfileByUserIdQuery(userId);
        var existingProfile = await profileQueryService.Handle(profileQuery);

        var isValid = (existingProfile != null) ? true : false;
        return isValid;
    }
}
