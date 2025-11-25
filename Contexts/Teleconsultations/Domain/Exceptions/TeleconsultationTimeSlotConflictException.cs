// Contexts/Teleconsultations/Domain/Exceptions/TeleconsultationTimeSlotConflictException.cs
namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Exceptions;

/// <summary>
/// Exception thrown when a teleconsultation time slot is already occupied.
/// </summary>
public sealed class TeleconsultationTimeSlotConflictException : TeleconsultationBusinessRuleException
{
    public string Date { get; }
    public string Time { get; }
    public int ExistingTeleconsultationId { get; }

    public TeleconsultationTimeSlotConflictException(string date, string time, int existingTeleconsultationId)
        : base($"A teleconsultation is already scheduled for {date} at {time}")
    {
        Date = date;
        Time = time;
        ExistingTeleconsultationId = existingTeleconsultationId;
    }
}