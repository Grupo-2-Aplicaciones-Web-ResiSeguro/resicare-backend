namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Resources;

public record TeleconsultationResource
{
    public int Id { get; init; }
    public string Service { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public string Time { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int UserId { get; init; } 
}