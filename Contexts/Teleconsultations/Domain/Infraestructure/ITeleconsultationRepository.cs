// Contexts/Teleconsultations/Domain/Infraestructure/ITeleconsultationRepository.cs
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Infraestructure;

public interface ITeleconsultationRepository : IBaseRepository<Teleconsultation>
{
    /// <summary>
    /// Retrieves all teleconsultations.
    /// </summary>
    /// <returns>A collection of all teleconsultations.</returns>
    Task<IEnumerable<Teleconsultation>> GetAllAsync();

    /// <summary>
    /// Retrieves teleconsultations by user identifier.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of teleconsultations for the specified user.</returns>
    Task<IEnumerable<Teleconsultation>> GetByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves teleconsultations by service type.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <returns>A collection of teleconsultations for the specified service.</returns>
    Task<IEnumerable<Teleconsultation>> GetByServiceAsync(string service);

    /// <summary>
    /// Finds a teleconsultation for a specific user at a specific date and time.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="date">The appointment date.</param>
    /// <param name="time">The appointment time.</param>
    /// <returns>The teleconsultation if found, otherwise null.</returns>
    Task<Teleconsultation?> FindByUserDateAndTimeAsync(int userId, string date, string time);

    /// <summary>
    /// Counts teleconsultations for a specific service at a specific date and time.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <param name="date">The appointment date.</param>
    /// <param name="time">The appointment time.</param>
    /// <returns>The number of teleconsultations for that service slot.</returns>
    Task<int> CountByServiceDateAndTimeAsync(string service, string date, string time);

    /// <summary>
    /// Counts active teleconsultations for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The number of active teleconsultations.</returns>
    Task<int> CountActiveTeleconsultationsByUserAsync(int userId);
}