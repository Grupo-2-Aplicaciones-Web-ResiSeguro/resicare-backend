using learning_center_webapi.Contexts.IAM.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.IAM.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResource(User user)
    {
        return new UserResource
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email.Value,
            Rol = user.Role.Value,
            CreatedAt = user.CreatedDate.ToString("o")
        };
    }
}