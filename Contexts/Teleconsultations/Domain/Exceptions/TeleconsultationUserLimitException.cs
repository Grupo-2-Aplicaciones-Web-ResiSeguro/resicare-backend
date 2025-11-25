// Contexts/Teleconsultations/Domain/Exceptions/TeleconsultationUserLimitException.cs
namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Exceptions;

/// <summary>
/// Exception thrown when a user exceeds the maximum number of active teleconsultations.
/// </summary>
public sealed class TeleconsultationUserLimitException : TeleconsultationBusinessRuleException
{
    public int UserId { get; }
    public int CurrentCount { get; }
    public int MaxAllowed { get; }

    public TeleconsultationUserLimitException(int userId, int currentCount, int maxAllowed)
        : base($"User has reached the maximum number of active teleconsultations ({maxAllowed})")
    {
        UserId = userId;
        CurrentCount = currentCount;
        MaxAllowed = maxAllowed;
    }
}