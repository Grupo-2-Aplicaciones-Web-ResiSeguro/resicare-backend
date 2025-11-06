namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Queries;

public record GetAllRegisteredObjectsQuery(string? Search, int? UserId);
