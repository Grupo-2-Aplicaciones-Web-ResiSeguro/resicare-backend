// Contexts/Claims/Domain/Exceptions/ClaimOutsideReportingPeriodException.cs
namespace learning_center_webapi.Contexts.Claims.Domain.Exceptions;

/// <summary>
/// Exception thrown when the incident date exceeds the maximum reporting period.
/// </summary>
public class ClaimOutsideReportingPeriodException : Exception
{
    public DateTime IncidentDate { get; }
    public int MaxDaysAllowed { get; }

    public ClaimOutsideReportingPeriodException(DateTime incidentDate, int maxDaysAllowed = 30)
        : base($"Incident date {incidentDate:yyyy-MM-dd} exceeds the maximum reporting period of {maxDaysAllowed} days")
    {
        IncidentDate = incidentDate;
        MaxDaysAllowed = maxDaysAllowed;
    }
}