namespace learning_center_webapi.Contexts.Claims.Domain.Commands;

public record UpdateClaimCommand(
    int ClaimId,
    string? Status,
    string? Description,
    int? RegisteredObjectId,
    int? Rating
);
