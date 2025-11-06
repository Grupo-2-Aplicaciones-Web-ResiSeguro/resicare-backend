namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Queries;

public class GetTeleconsultationById
{
    public GetTeleconsultationById(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}