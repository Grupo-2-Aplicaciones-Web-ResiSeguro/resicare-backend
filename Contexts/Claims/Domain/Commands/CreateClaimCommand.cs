namespace learning_center_webapi.Contexts.Claims.Domain.Commands;

public record DocumentPayload(string Name, string ContentType, long Size, string Data);

public record CreateClaimCommand(
    string Type,
    DateTime IncidentDate,
    string Description,
    int? RegisteredObjectId,
    IEnumerable<DocumentPayload> Documents,
    int UserId
);
