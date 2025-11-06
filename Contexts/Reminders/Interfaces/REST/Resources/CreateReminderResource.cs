namespace learning_center_webapi.Contexts.Reminders.Interfaces.REST.Resources;

public record CreateReminderResource
{
    public required int UserId { get; init; }
    public required string Title { get; init; }
    public required string Type { get; init; }
    public required string Date { get; init; }
    public required string Time { get; init; }
    public string? Notes { get; init; }
}