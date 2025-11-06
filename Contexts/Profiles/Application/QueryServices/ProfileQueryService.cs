
using learning_center_webapi.Contexts.Profiles.Domain.Infraestructure;
using learning_center_webapi.Contexts.Profiles.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Profiles.Domain.Queries;

namespace learning_center_webapi.Contexts.Profiles.Application.QueryServices;

public class ProfileQueryService
{
    private readonly IProfileRepository _profileRepository;

    public ProfileQueryService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query)
    {
        return await _profileRepository.GetAllAsync();
    }

    public async Task<Profile?> Handle(GetProfileByIdQuery query)
    {
        return await _profileRepository.FindByIdAsync(query.Id);
    }

    public async Task<Profile?> Handle(GetProfileByUserIdQuery query)
    {
        return await _profileRepository.FindByUserIdAsync(query.UserId);
    }
}