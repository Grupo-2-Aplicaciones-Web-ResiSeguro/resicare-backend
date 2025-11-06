namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;

public class CreateTeleconsultationCommand
{
    public required string Service { get; set; }
    public required string Date { get; set; }
    public required string Time { get; set; } 
    public required string Description { get; set; }
    public required int UserId { get; set; }
}