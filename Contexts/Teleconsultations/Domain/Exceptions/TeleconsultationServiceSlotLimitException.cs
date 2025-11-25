// Contexts/Teleconsultations/Domain/Exceptions/TeleconsultationServiceSlotLimitException.cs
namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Exceptions;

/// <summary>
/// Exception thrown when the service slot has reached its maximum capacity.
/// </summary>
public sealed class TeleconsultationServiceSlotLimitException : TeleconsultationBusinessRuleException
{
    public string Date { get; }
    public string Time { get; }
    public string Service { get; }
    public int CurrentCount { get; }
    public int MaxAllowed { get; }

    public TeleconsultationServiceSlotLimitException(string date, string time, string service, int currentCount, int maxAllowed)
        : base($"The service '{service}' has reached its maximum capacity ({maxAllowed}) for {date} at {time}")
    {
        Date = date;
        Time = time;
        Service = service;
        CurrentCount = currentCount;
        MaxAllowed = maxAllowed;
    }
}