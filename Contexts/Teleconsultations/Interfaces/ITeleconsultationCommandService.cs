using learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces;

public interface ITeleconsultationCommandService
{
    Task<Teleconsultation> Handle(CreateTeleconsultationCommand command);
    Task<Teleconsultation> Handle(UpdateTeleconsultationCommand command);
    Task<bool> Handle(DeleteTeleconsultationCommand command);
}