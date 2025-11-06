
using learning_center_webapi.Contexts.IAM.Domain.Infraestructure;
using learning_center_webapi.Contexts.IAM.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.IAM.Domain.Queries;

namespace learning_center_webapi.Contexts.IAM.Application.QueryServices;

public class UserQueryService
{
    private readonly IUserRepository _userRepository;

    public UserQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await _userRepository.FindByIdAsync(query.Id);
    }

    public async Task<User?> Handle(GetUserByEmailQuery query)
    {
        return await _userRepository.FindByEmailAsync(query.Email);
    }
}