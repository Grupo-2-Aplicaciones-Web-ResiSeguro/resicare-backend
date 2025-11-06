namespace learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;

public record UpdateClaimResource(
    string? Status,
    string? Description,
    int? RegisteredObjectId,
    int? Rating
);
