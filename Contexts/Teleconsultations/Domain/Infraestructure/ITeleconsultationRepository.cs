
using learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Infraestructure;

public interface ITeleconsultationRepository : IBaseRepository<Teleconsultation>
{
    Task<IEnumerable<Teleconsultation>> GetAllAsync();
    Task<IEnumerable<Teleconsultation>> GetByUserIdAsync(int userId);  
    Task<IEnumerable<Teleconsultation>> GetByServiceAsync(string service);
}