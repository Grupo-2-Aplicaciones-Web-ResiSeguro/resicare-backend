namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Queries;

public class GetTeleconsultationsByService
{
    public GetTeleconsultationsByService(string service)
    {
        Service = service;
    }

    public string Service { get; set; }
}