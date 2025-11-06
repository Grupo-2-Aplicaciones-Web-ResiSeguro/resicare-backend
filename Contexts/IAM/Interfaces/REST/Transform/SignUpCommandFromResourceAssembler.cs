using learning_center_webapi.Contexts.IAM.Domain.Commands;
using learning_center_webapi.Contexts.IAM.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommand(SignUpResource resource)
    {
        return new SignUpCommand
        {
            Name = resource.Name,
            Email = resource.Email,
            Password = resource.Password,
            Rol = resource.Rol
        };
    }
}