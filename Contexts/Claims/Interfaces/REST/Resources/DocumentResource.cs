namespace learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;

public record DocumentResource(
    int Id,
    string Name,
    string ContentType,
    long Size,
    string Data
);
