namespace learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;

public record CreateClaimResource(
    string Type,
    DateTime IncidentDate,
    string Description,
    int? RegisteredObjectId,
    IEnumerable<CreateDocumentResource> Documents,
    int UserId
);

public record CreateDocumentResource(
    string Name,
    string ContentType,
    long Size,
    string Data
);
