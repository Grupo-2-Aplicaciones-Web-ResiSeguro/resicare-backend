
using learning_center_webapi.Contexts.IAM.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.IAM.Domain.Infraestructure;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> FindByEmailAsync(string email);
}