using System.Linq;
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Claims.Interfaces.REST.Transform;

public static class ClaimResourceFromEntityAssembler
{
    public static ClaimResource ToResource(Claim claim)
    {
        var documents = claim.Documents
            .Select(document => new DocumentResource(
                document.Id,
                document.Name,
                document.ContentType,
                document.Size,
                document.Data
            ))
            .ToList();

        return new ClaimResource(
            claim.Id,
            claim.Number,
            claim.Type,
            claim.Status,
            claim.IncidentDate,
            claim.CreatedDate,
            claim.Description,
            claim.RegisteredObjectId,
            documents,
            claim.UserId,
            claim.Rating
        );
    }
}
