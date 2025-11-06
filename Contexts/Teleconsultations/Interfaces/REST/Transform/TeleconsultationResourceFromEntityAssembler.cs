using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Transform;

public static class TeleconsultationResourceFromEntityAssembler
{
    public static TeleconsultationResource ToResource(Teleconsultation entity)
    {
        return new TeleconsultationResource
        {
            Id = entity.Id,
            Service = entity.Service,
            Date = entity.Date,
            Time = entity.Time,
            Description = entity.Description,
            UserId = entity.UserId
        };
    }
}