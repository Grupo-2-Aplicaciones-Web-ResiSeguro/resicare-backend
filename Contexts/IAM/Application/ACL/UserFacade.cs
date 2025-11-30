using learning_center_webapi.Contexts.IAM.Application.QueryServices;
using learning_center_webapi.Contexts.IAM.Domain.Queries;
using learning_center_webapi.Contexts.IAM.Interfaces.REST.ACL;

namespace learning_center_webapi.Contexts.IAM.Application.ACL;

public class UserFacade(UserQueryService userQueryService) : IUserFacade
{
    public async Task<bool> IsValidUserId(int userId)
    {
        var userQuery = new GetUserByIdQuery(userId);
        var existingUser = await userQueryService.Handle(userQuery);

        var isValid = (existingUser != null) ? true : false;
        return isValid;
    }
}
