
namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Exceptions;

public sealed class InvalidAppointmentDateException : TeleconsultationBusinessRuleException
{
    public InvalidAppointmentDateException(string message)
        : base(message) { }

    public static InvalidAppointmentDateException HourNotAllowed(int hour)
        => new InvalidAppointmentDateException($"The hour '{hour}' is not allowed. Allowed hours are 9, 11, 14 and 16.");

    public static InvalidAppointmentDateException DateInPast(DateTime date)
        => new InvalidAppointmentDateException($"The appointment date '{date:yyyy-MM-dd HH:mm}' is in the past.");

    public static InvalidAppointmentDateException InvalidCombination(DateTime date, int hour)
        => new InvalidAppointmentDateException($"Invalid appointment date/time: {date:yyyy-MM-dd} at hour {hour}.");
}