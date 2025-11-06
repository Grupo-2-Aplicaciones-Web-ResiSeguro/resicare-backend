namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Resources;

public record CreateTeleconsultationResource
{
    public required string Service { get; init; }
    public required string Date { get; init; }
    public required string Time { get; init; }
    public required string Description { get; init; }
    public required int UserId { get; init; }
}