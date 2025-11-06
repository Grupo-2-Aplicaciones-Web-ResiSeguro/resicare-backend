using learning_center_webapi.Contexts.Claims.Domain.Commands;
using learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Claims.Interfaces.REST.Transform;

public static class UpdateClaimCommandFromResourceAssembler
{
    public static UpdateClaimCommand ToCommand(int claimId, UpdateClaimResource resource)
    {
        return new UpdateClaimCommand(
            claimId,
            resource.Status,
            resource.Description,
            resource.RegisteredObjectId,
            resource.Rating
        );
    }
}
