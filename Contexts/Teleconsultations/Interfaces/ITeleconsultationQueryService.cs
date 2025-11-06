
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Queries;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces;

public interface ITeleconsultationQueryService
{
    Task<IEnumerable<Teleconsultation>> Handle(GetAllTeleconsultations query);
    Task<Teleconsultation?> Handle(GetTeleconsultationById query);
    Task<IEnumerable<Teleconsultation>> Handle(GetTeleconsultationsByUserId query);
    Task<IEnumerable<Teleconsultation>> Handle(GetTeleconsultationsByService query);
}

