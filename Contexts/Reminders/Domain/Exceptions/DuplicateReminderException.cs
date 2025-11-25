namespace learning_center_webapi.Contexts.Reminders.Domain.Exceptions;
public sealed class DuplicateReminderException : ReminderBusinessRuleException
{
    public int UserId { get; }
    public string Title { get; }
    public string Date { get; }
    public string Time { get; }
    public int ExistingReminderId { get; }

    public DuplicateReminderException(int userId, string title, string date, string time, int existingReminderId) 
        : base("A reminder with the same details already exists")
    {
        UserId = userId;
        Title = title;
        Date = date;
        Time = time;
        ExistingReminderId = existingReminderId;
    }
}