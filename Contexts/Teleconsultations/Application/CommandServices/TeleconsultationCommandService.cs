// Contexts/Teleconsultations/Application/CommandServices/TeleconsultationCommandService.cs
using System.Globalization;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Exceptions;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Infraestructure;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Teleconsultations.Application.CommandServices;

/// <summary>
/// Service responsible for handling teleconsultation-related commands.
/// </summary>
public class TeleconsultationCommandService
{
    private readonly ITeleconsultationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    // Servicios válidos tal como llegan del frontend
    private static readonly Dictionary<string, string> AllowedServices = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Nutrition", "nutrition" },
        { "General Medicine", "general" },
        { "Psychology", "psychology" }
    };

    private static readonly int[] AllowedHours = { 9, 11, 14, 16 };
    private const int MaxActiveTeleconsultationsPerUser = 2;
    private const int MaxTeleconsultationsPerServiceSlot = 3;

    public TeleconsultationCommandService(ITeleconsultationRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the creation of a new teleconsultation.
    /// </summary>
    /// <param name="command">Command containing teleconsultation creation data.</param>
    /// <returns>The created teleconsultation entity.</returns>
    /// <exception cref="InvalidServiceTypeException">Thrown when service type is invalid.</exception>
    /// <exception cref="InvalidAppointmentDateException">Thrown when date/time is invalid.</exception>
    /// <exception cref="TeleconsultationTimeSlotConflictException">Thrown when user already has a teleconsultation at that time.</exception>
    /// <exception cref="TeleconsultationServiceSlotLimitException">Thrown when service slot is full.</exception>
    /// <exception cref="TeleconsultationUserLimitException">Thrown when user exceeds active teleconsultations limit.</exception>
    public async Task<Teleconsultation> Handle(CreateTeleconsultationCommand command)
    {
        // Validate and normalize service type
        var normalizedService = ValidateAndNormalizeServiceType(command.Service);

        // Validate date and time
        ValidateDateAndTime(command.Date, command.Time);

        // Check if user already has a teleconsultation at this date/time
        await ValidateUserTimeSlotConflict(command.UserId, command.Date, command.Time);

        // Check if service slot has reached its limit
        await ValidateServiceSlotLimit(normalizedService, command.Date, command.Time);

        // Check user's active teleconsultations limit
        await ValidateUserTeleconsultationLimit(command.UserId);

        var teleconsultation = new Teleconsultation(
            normalizedService,
            command.Date,
            command.Time,
            command.Description,
            command.UserId
        );

        await _repository.AddAsync(teleconsultation);
        await _unitOfWork.CompleteAsync();
        return teleconsultation;
    }

    /// <summary>
    /// Handles the update of an existing teleconsultation.
    /// </summary>
    /// <param name="command">Command containing teleconsultation update data.</param>
    /// <returns>The updated teleconsultation entity.</returns>
    /// <exception cref="TeleconsultationNotFoundException">Thrown when teleconsultation is not found.</exception>
    /// <exception cref="InvalidServiceTypeException">Thrown when service type is invalid.</exception>
    /// <exception cref="InvalidAppointmentDateException">Thrown when date/time is invalid.</exception>
    /// <exception cref="TeleconsultationTimeSlotConflictException">Thrown when user already has a teleconsultation at that time.</exception>
    /// <exception cref="TeleconsultationServiceSlotLimitException">Thrown when service slot is full.</exception>
    public async Task<Teleconsultation> Handle(UpdateTeleconsultationCommand command)
    {
        var teleconsultation = await _repository.FindByIdAsync(command.Id);

        if (teleconsultation == null)
            throw new TeleconsultationNotFoundException(command.Id.ToString());

        var originalService = teleconsultation.Service;
        var originalDate = teleconsultation.Date;
        var originalTime = teleconsultation.Time;

        // Validate and normalize service if being changed
        if (!string.IsNullOrEmpty(command.Service) && command.Service != teleconsultation.Service)
        {
            var normalizedService = ValidateAndNormalizeServiceType(command.Service);
            teleconsultation.Service = normalizedService;
        }

        // Validate date/time if being changed
        var dateChanged = !string.IsNullOrEmpty(command.Date) && command.Date != originalDate;
        var timeChanged = !string.IsNullOrEmpty(command.Time) && command.Time != originalTime;
        var serviceChanged = teleconsultation.Service != originalService;

        if (dateChanged || timeChanged || serviceChanged)
        {
            var newDate = command.Date ?? originalDate;
            var newTime = command.Time ?? originalTime;
            var newService = teleconsultation.Service;

            ValidateDateAndTime(newDate, newTime);

            // Check if user already has another teleconsultation at this new time
            await ValidateUserTimeSlotConflict(teleconsultation.UserId, newDate, newTime, command.Id);

            // Check if service slot has capacity (excluding current teleconsultation)
            await ValidateServiceSlotLimit(newService, newDate, newTime, command.Id);

            if (dateChanged) teleconsultation.Date = newDate;
            if (timeChanged) teleconsultation.Time = newTime;
        }

        if (!string.IsNullOrEmpty(command.Description))
            teleconsultation.Description = command.Description;

        _repository.Update(teleconsultation);
        await _unitOfWork.CompleteAsync();
        return teleconsultation;
    }

    /// <summary>
    /// Handles the deletion of a teleconsultation.
    /// </summary>
    /// <param name="command">Command containing teleconsultation identifier.</param>
    /// <returns>True if deleted successfully.</returns>
    /// <exception cref="TeleconsultationNotFoundException">Thrown when teleconsultation is not found.</exception>
    public async Task<bool> Handle(DeleteTeleconsultationCommand command)
    {
        var teleconsultation = await _repository.FindByIdAsync(command.Id);

        if (teleconsultation == null)
            throw new TeleconsultationNotFoundException(command.Id.ToString());

        _repository.Remove(teleconsultation);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    /// <summary>
    /// Validates that the service type is allowed and normalizes it to database format.
    /// </summary>
    /// <param name="service">The service type to validate (from frontend).</param>
    /// <returns>The normalized service name for database storage.</returns>
    /// <exception cref="InvalidServiceTypeException">Thrown when service type is invalid.</exception>
    private static string ValidateAndNormalizeServiceType(string service)
    {
        if (AllowedServices.TryGetValue(service, out var normalizedService))
        {
            return normalizedService;
        }

        // Si no se encuentra, lanzar excepción con los nombres que acepta el frontend
        throw new InvalidServiceTypeException(service, AllowedServices.Keys.ToArray());
    }

    /// <summary>
    /// Validates that the appointment date and time are valid.
    /// </summary>
    /// <param name="date">The appointment date in format yyyy-MM-dd.</param>
    /// <param name="time">The appointment time (hour).</param>
    /// <exception cref="InvalidAppointmentDateException">Thrown when date/time is invalid.</exception>
    private static void ValidateDateAndTime(string date, string time)
    {
        // Parse date
        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var appointmentDate))
        {
            throw InvalidAppointmentDateException.InvalidCombination(DateTime.MinValue, 0);
        }

        // Parse hour from time string (could be "9", "09:00", "9:00", etc.)
        var timeParts = time.Split(':');
        if (!int.TryParse(timeParts[0], out var hour))
        {
            throw InvalidAppointmentDateException.HourNotAllowed(0);
        }

        // Validate hour is in allowed list
        if (!AllowedHours.Contains(hour))
        {
            throw InvalidAppointmentDateException.HourNotAllowed(hour);
        }

        // Validate date is not in the past
        var appointmentDateTime = appointmentDate.Date.AddHours(hour);
        if (appointmentDateTime < DateTime.UtcNow)
        {
            throw InvalidAppointmentDateException.DateInPast(appointmentDateTime);
        }
    }

    /// <summary>
    /// Validates that the user doesn't already have a teleconsultation at the specified date/time.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="date">The appointment date.</param>
    /// <param name="time">The appointment time.</param>
    /// <param name="excludeId">Optional teleconsultation ID to exclude (for updates).</param>
    /// <exception cref="TeleconsultationTimeSlotConflictException">Thrown when user already has a teleconsultation at that time.</exception>
    private async Task ValidateUserTimeSlotConflict(int userId, string date, string time, int? excludeId = null)
    {
        var existingTeleconsultation = await _repository.FindByUserDateAndTimeAsync(userId, date, time);

        if (existingTeleconsultation != null && existingTeleconsultation.Id != excludeId)
        {
            throw new TeleconsultationTimeSlotConflictException(date, time, existingTeleconsultation.Id);
        }
    }

    /// <summary>
    /// Validates that the service slot hasn't reached its maximum capacity.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="date">The appointment date.</param>
    /// <param name="time">The appointment time.</param>
    /// <param name="excludeId">Optional teleconsultation ID to exclude (for updates).</param>
    /// <exception cref="TeleconsultationServiceSlotLimitException">Thrown when service slot is full.</exception>
    private async Task ValidateServiceSlotLimit(string service, string date, string time, int? excludeId = null)
    {
        var count = await _repository.CountByServiceDateAndTimeAsync(service, date, time);

        // If updating, don't count the current teleconsultation
        if (excludeId.HasValue)
        {
            var current = await _repository.FindByIdAsync(excludeId.Value);
            if (current != null && current.Service == service && current.Date == date && current.Time == time)
            {
                count--;
            }
        }

        if (count >= MaxTeleconsultationsPerServiceSlot)
        {
            throw new TeleconsultationServiceSlotLimitException(date, time, service, count, MaxTeleconsultationsPerServiceSlot);
        }
    }

    /// <summary>
    /// Validates that the user has not exceeded the maximum active teleconsultations limit.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <exception cref="TeleconsultationUserLimitException">Thrown when user exceeds the limit.</exception>
    private async Task ValidateUserTeleconsultationLimit(int userId)
    {
        var activeCount = await _repository.CountActiveTeleconsultationsByUserAsync(userId);

        if (activeCount >= MaxActiveTeleconsultationsPerUser)
        {
            throw new TeleconsultationUserLimitException(userId, activeCount, MaxActiveTeleconsultationsPerUser);
        }
    }
}