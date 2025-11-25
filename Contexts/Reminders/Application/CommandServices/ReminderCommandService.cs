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
    private const int MaxDaysInFuture = 180; // 6 months

    public ReminderCommandService(IReminderRepository reminderRepository, IUnitOfWork unitOfWork)
    {
        _reminderRepository = reminderRepository;
        _unitOfWork = unitOfWork;
    }

    /// <exception cref="InvalidReminderTitleException">title invalid</exception>
    /// <exception cref="InvalidReminderDateFormatException">date format invalid</exception>
    /// <exception cref="InvalidReminderTimeFormatException">time format is invalid</exception>
    /// <exception cref="ReminderInPastException">reminder date/time is in the past</exception>
    /// <exception cref="ReminderTooFarInFutureException">reminder is too far in the future</exception>
    /// <exception cref="DuplicateReminderException">duplicate reminder exists</exception>
    public async Task<Reminder> Handle(CreateReminderCommand command)
    {
        var title = new ReminderTitle(command.Title);
        var type = new ReminderType(command.Type);
        var date = new ReminderDate(command.Date);
        var time = new ReminderTime(command.Time);

        var reminderDateTime = date.ToDateTime().Add(time.ToTimeSpan());
        if (reminderDateTime < DateTime.Now)
            throw new ReminderInPastException(reminderDateTime);

        var maxFutureDate = DateTime.Now.AddDays(MaxDaysInFuture);
        if (reminderDateTime > maxFutureDate)
            throw new ReminderTooFarInFutureException(reminderDateTime, MaxDaysInFuture);

        // Check for duplicate reminders
        var duplicate = await _reminderRepository.FindDuplicateAsync(
            command.UserId, 
            title.Value, 
            date.Value, 
            time.Value, 
            type.Value);

        if (duplicate != null)
            throw new DuplicateReminderException(
                command.UserId, 
                title.Value, 
                date.Value, 
                time.Value, 
                duplicate.Id);

        var reminder = new Reminder(
            command.UserId,
            title,
            type,
            date,
            time,
            command.Notes
        );

        await _reminderRepository.AddAsync(reminder);
        await _unitOfWork.CompleteAsync();

        return reminder;
    }

    /// <summary>
    /// Handles the update of an existing reminder
    /// </summary>
    /// <param name="command">command containing reminder update data</param>
    /// <returns>updated reminder entity</returns>

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

            // Validate reminder is not in the past
            var reminderDateTime = date.ToDateTime().Add(time.ToTimeSpan());
            if (reminderDateTime < DateTime.Now)
                throw new ReminderInPastException(reminderDateTime);

            // Validate reminder is not too far in future
            var maxFutureDate = DateTime.Now.AddDays(MaxDaysInFuture);
            if (reminderDateTime > maxFutureDate)
                throw new ReminderTooFarInFutureException(reminderDateTime, MaxDaysInFuture);

            var duplicate = await _reminderRepository.FindDuplicateAsync(
                reminder.UserId, 
                title.Value, 
                date.Value, 
                time.Value, 
                type.Value);

            if (duplicate != null && duplicate.Id != command.Id)
                throw new DuplicateReminderException(
                    reminder.UserId, 
                    title.Value, 
                    date.Value, 
                    time.Value, 
                    duplicate.Id);
            
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

    /// <summary>
    /// Handles the deletion of a reminder.
    /// </summary>
    /// <param name="command">Command containing reminder identifier.</param>
    /// <returns>True if deleted successfully.</returns>
    public async Task<bool> Handle(DeleteReminderCommand command)
    {
        var reminder = await _reminderRepository.FindByIdAsync(command.Id);
        
        if (reminder == null)
            throw new ReminderNotFoundException(command.Id);

        _reminderRepository.Remove(reminder);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}