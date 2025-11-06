namespace learning_center_webapi.Contexts.Reminders.Interfaces.REST.Resources;

public record UpdateReminderResource
{
    public string? Title { get; init; }
    public string? Type { get; init; }
    public string? Date { get; init; }
    public string? Time { get; init; }
    public string? Notes { get; init; }
}