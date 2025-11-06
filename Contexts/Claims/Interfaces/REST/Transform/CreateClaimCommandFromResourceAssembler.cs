using System.Linq;
using learning_center_webapi.Contexts.Claims.Domain.Commands;
using learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Claims.Interfaces.REST.Transform;

public static class CreateClaimCommandFromResourceAssembler
{
    public static CreateClaimCommand ToCommand(CreateClaimResource resource)
    {
        var documents = resource.Documents?.Select(document => new DocumentPayload(
            document.Name,
            document.ContentType,
            document.Size,
            document.Data
        )) ?? Enumerable.Empty<DocumentPayload>();

        return new CreateClaimCommand(
            resource.Type,
            resource.IncidentDate,
            resource.Description,
            resource.RegisteredObjectId,
            documents,
            resource.UserId
        );
    }
}
