using learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Shared.Domain.Model.Entities;
using System;

namespace learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;

public class Reminder : BaseEntity
{
    public int UserId { get; set; }
    public ReminderTitle Title { get; private set; } = default!;
    public ReminderType Type { get; private set; } = default!;
    public ReminderDate Date { get; private set; } = default!;
    public ReminderTime Time { get; private set; } = default!;
    public string? Notes { get; set; }

    // Constructor para EF Core
    public Reminder() { }

    // Constructor para creación
    public Reminder(
        int userId,
        ReminderTitle title,
        ReminderType type,
        ReminderDate date,
        ReminderTime time,
        string? notes = null)
    {
        UserId = userId;
        Title = title;
        Type = type;
        Date = date;
        Time = time;
        Notes = notes;
    }

    public void ValidateNotInPast()
    {
        var reminderDateTime = Date.ToDateTime().Add(Time.ToTimeSpan());
        if (reminderDateTime < DateTime.Now)
            throw new ArgumentException("Cannot create reminders in the past");
    }

    public void UpdateDetails(ReminderTitle title, ReminderType type, ReminderDate date, ReminderTime time)
    {
        Title = title;
        Type = type;
        Date = date;
        Time = time;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes;
    }

    public bool IsPast()
    {
        var reminderDateTime = Date.ToDateTime().Add(Time.ToTimeSpan());
        return reminderDateTime < DateTime.Now;
    }

    public bool IsUpcoming()
    {
        var reminderDateTime = Date.ToDateTime().Add(Time.ToTimeSpan());
        var now = DateTime.Now;
        var tomorrow = now.AddHours(24);
        return reminderDateTime >= now && reminderDateTime <= tomorrow;
    }
}