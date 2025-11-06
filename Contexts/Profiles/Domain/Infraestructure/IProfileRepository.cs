
using learning_center_webapi.Contexts.Profiles.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Profiles.Domain.Infraestructure;

public interface IProfileRepository : IBaseRepository<Profile>
{
    Task<IEnumerable<Profile>> GetAllAsync();
    Task<Profile?> FindByUserIdAsync(int userId);
}