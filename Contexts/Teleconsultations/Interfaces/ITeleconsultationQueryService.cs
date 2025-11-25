// Contexts/Teleconsultations/Interfaces/ITeleconsultationQueryService.cs
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Queries;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces;

/// <summary>
/// Interface for teleconsultation query operations.
/// </summary>
public interface ITeleconsultationQueryService
{
    /// <summary>
    /// Retrieves all teleconsultations.
    /// </summary>
    /// <param name="query">Query for all teleconsultations.</param>
    /// <returns>A collection of all teleconsultations.</returns>
    Task<IEnumerable<Teleconsultation>> Handle(GetAllTeleconsultations query);

    /// <summary>
    /// Retrieves a teleconsultation by its identifier.
    /// </summary>
    /// <param name="query">Query containing teleconsultation identifier.</param>
    /// <returns>The teleconsultation if found, otherwise null.</returns>
    Task<Teleconsultation?> Handle(GetTeleconsultationById query);

    /// <summary>
    /// Retrieves teleconsultations by user identifier.
    /// </summary>
    /// <param name="query">Query containing user identifier.</param>
    /// <returns>A collection of teleconsultations for the specified user.</returns>
    Task<IEnumerable<Teleconsultation>> Handle(GetTeleconsultationsByUserId query);

    /// <summary>
    /// Retrieves teleconsultations by service type.
    /// </summary>
    /// <param name="query">Query containing service type.</param>
    /// <returns>A collection of teleconsultations for the specified service.</returns>
    Task<IEnumerable<Teleconsultation>> Handle(GetTeleconsultationsByService query);
}