// Contexts/Claims/Domain/Exceptions/InvalidClaimStatusTransitionException.cs
namespace learning_center_webapi.Contexts.Claims.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting an invalid claim status transition.
/// </summary>
public class InvalidClaimStatusTransitionException : Exception
{
    public string CurrentStatus { get; }
    public string NewStatus { get; }

    public InvalidClaimStatusTransitionException(string currentStatus, string newStatus)
        : base($"Invalid status transition from '{currentStatus}' to '{newStatus}'")
    {
        CurrentStatus = currentStatus;
        NewStatus = newStatus;
    }
}