// Contexts/Teleconsultations/Domain/Exceptions/InvalidServiceTypeException.cs
namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Exceptions;

/// <summary>
/// Exception thrown when an invalid service type is provided.
/// </summary>
public sealed class InvalidServiceTypeException : TeleconsultationBusinessRuleException
{
    public string ProvidedService { get; }
    public string[] AllowedServices { get; }

    public InvalidServiceTypeException(string service, string[] allowedServices)
        : base($"Service type '{service}' is not valid. Allowed services are: {string.Join(", ", allowedServices)}")
    {
        ProvidedService = service;
        AllowedServices = allowedServices;
    }
}