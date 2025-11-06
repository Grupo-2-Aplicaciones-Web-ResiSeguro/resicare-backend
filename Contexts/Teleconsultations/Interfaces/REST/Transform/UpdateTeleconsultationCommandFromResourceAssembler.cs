using learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;
using learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Transform;

public static class UpdateTeleconsultationCommandFromResourceAssembler
{
    public static UpdateTeleconsultationCommand ToCommand(int id, UpdateTeleconsultationResource resource)
    {
        return new UpdateTeleconsultationCommand
        {
            Id = id,
            Service = resource.Service,
            Date = resource.Date,
            Time = resource.Time,
            Description = resource.Description
        };
    }
}