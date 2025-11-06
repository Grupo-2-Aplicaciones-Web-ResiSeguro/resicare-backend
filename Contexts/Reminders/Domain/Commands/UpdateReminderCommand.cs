namespace learning_center_webapi.Contexts.Reminders.Domain.Commands;

public record UpdateReminderCommand
{
    public required int Id { get; init; }
    public string? Title { get; init; }
    public string? Type { get; init; }
    public string? Date { get; init; }
    public string? Time { get; init; }
    public string? Notes { get; init; }
}