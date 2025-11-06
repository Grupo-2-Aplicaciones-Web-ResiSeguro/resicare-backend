using learning_center_webapi.Contexts.IAM.Domain.Commands;
using learning_center_webapi.Contexts.IAM.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommand(SignInResource resource)
    {
        return new SignInCommand
        {
            Email = resource.Email,
            Password = resource.Password
        };
    }
}