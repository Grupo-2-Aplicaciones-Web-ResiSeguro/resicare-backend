using learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;
using learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Transform;

public static class CreateTeleconsultationCommandFromResourceAssembler
{
    public static CreateTeleconsultationCommand ToCommand(CreateTeleconsultationResource resource)
    {
        return new CreateTeleconsultationCommand
        {
            Service = resource.Service,
            Date = resource.Date,
            Time = resource.Time,
            Description = resource.Description,
            UserId = resource.UserId
        };
    }
}