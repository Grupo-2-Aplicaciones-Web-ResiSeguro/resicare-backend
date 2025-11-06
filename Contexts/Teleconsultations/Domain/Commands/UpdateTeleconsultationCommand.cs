namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;

public class UpdateTeleconsultationCommand
{
    public required int Id { get; set; }
    public string? Service { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? Description { get; set; }
}