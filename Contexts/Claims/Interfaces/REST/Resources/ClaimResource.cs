namespace learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;

public record ClaimResource(
    int Id,
    string Number,
    string Type,
    string Status,
    DateTime IncidentDate,
    DateTime CreatedDate,
    string Description,
    int? RegisteredObjectId,
    IReadOnlyCollection<DocumentResource> Documents,
    int UserId,
    int? Rating
);
