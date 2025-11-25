// Contexts/Teleconsultations/Interfaces/ITeleconsultationCommandService.cs
using learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces;

/// <summary>
/// Interface for teleconsultation command operations.
/// </summary>
public interface ITeleconsultationCommandService
{
    /// <summary>
    /// Handles the creation of a new teleconsultation.
    /// </summary>
    /// <param name="command">Command containing teleconsultation creation data.</param>
    /// <returns>The created teleconsultation entity.</returns>
    Task<Teleconsultation> Handle(CreateTeleconsultationCommand command);

    /// <summary>
    /// Handles the update of an existing teleconsultation.
    /// </summary>
    /// <param name="command">Command containing teleconsultation update data.</param>
    /// <returns>The updated teleconsultation entity.</returns>
    Task<Teleconsultation> Handle(UpdateTeleconsultationCommand command);

    /// <summary>
    /// Handles the deletion of a teleconsultation.
    /// </summary>
    /// <param name="command">Command containing teleconsultation identifier.</param>
    /// <returns>True if deleted successfully.</returns>
    Task<bool> Handle(DeleteTeleconsultationCommand command);
}