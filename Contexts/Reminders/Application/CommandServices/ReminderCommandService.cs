
using learning_center_webapi.Contexts.Reminders.Domain.Commands;
using learning_center_webapi.Contexts.Reminders.Domain.Exceptions;
using learning_center_webapi.Contexts.Reminders.Domain.Infraestructure;
using learning_center_webapi.Contexts.Reminders.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Reminders.Domain.Model.ValueObjects;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Reminders.Application.CommandServices;

public class ReminderCommandService
{
    private readonly IReminderRepository _reminderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReminderCommandService(IReminderRepository reminderRepository, IUnitOfWork unitOfWork)
    {
        _reminderRepository = reminderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Reminder> Handle(CreateReminderCommand command)
    {
        var title = new ReminderTitle(command.Title);
        var type = new ReminderType(command.Type);
        var date = new ReminderDate(command.Date);
        var time = new ReminderTime(command.Time);

        var reminder = new Reminder(
            command.UserId,
            title,
            type,
            date,
            time,
            command.Notes
        );

        try
        {
            reminder.ValidateNotInPast();
        }
        catch (ArgumentException)
        {
            throw new ReminderInPastException();
        }

        await _reminderRepository.AddAsync(reminder);
        await _unitOfWork.CompleteAsync();

        return reminder;
    }

    public async Task<Reminder> Handle(UpdateReminderCommand command)
    {
        var reminder = await _reminderRepository.FindByIdAsync(command.Id);
        
        if (reminder == null)
            throw new ReminderNotFoundException(command.Id);

        if (!string.IsNullOrEmpty(command.Title) || !string.IsNullOrEmpty(command.Type) ||
            !string.IsNullOrEmpty(command.Date) || !string.IsNullOrEmpty(command.Time))
        {
            var title = !string.IsNullOrEmpty(command.Title) ? new ReminderTitle(command.Title) : reminder.Title;
            var type = !string.IsNullOrEmpty(command.Type) ? new ReminderType(command.Type) : reminder.Type;
            var date = !string.IsNullOrEmpty(command.Date) ? new ReminderDate(command.Date) : reminder.Date;
            var time = !string.IsNullOrEmpty(command.Time) ? new ReminderTime(command.Time) : reminder.Time;
            
            reminder.UpdateDetails(title, type, date, time);
        }

        if (command.Notes != null)
        {
            reminder.UpdateNotes(command.Notes);
        }

        _reminderRepository.Update(reminder);
        await _unitOfWork.CompleteAsync();

        return reminder;
    }

    public async Task<bool> Handle(DeleteReminderCommand command)
    {
        var reminder = await _reminderRepository.FindByIdAsync(command.Id);
        
        if (reminder == null)
            return false;

        _reminderRepository.Remove(reminder);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}