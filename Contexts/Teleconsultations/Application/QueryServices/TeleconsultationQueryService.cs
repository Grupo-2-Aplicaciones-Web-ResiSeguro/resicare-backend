
using learning_center_webapi.Contexts.Teleconsultations.Domain.Infraestructure;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Queries;

namespace learning_center_webapi.Contexts.Teleconsultations.Application.QueryServices;

public class TeleconsultationQueryService
{
    private readonly ITeleconsultationRepository _repository;

    public TeleconsultationQueryService(ITeleconsultationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Teleconsultation>> Handle(GetAllTeleconsultations query)
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Teleconsultation?> Handle(GetTeleconsultationById query)
    {
        return await _repository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Teleconsultation>> Handle(GetTeleconsultationsByUserId query)
    {
        return await _repository.GetByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Teleconsultation>> Handle(GetTeleconsultationsByService query)
    {
        return await _repository.GetByServiceAsync(query.Service);
    }
}