using learning_center_webapi.Contexts.Shared.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Model.Entities;

public class Teleconsultation : BaseEntity
{
    public string Service { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; } 

    public Teleconsultation() { }

    public Teleconsultation(
        string service,
        string date,
        string time,
        string description,
        int userId) 
    {
        Service = service;
        Date = date;
        Time = time;
        Description = description;
        UserId = userId;
    }
}