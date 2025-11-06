using learning_center_webapi.Contexts.Profiles.Domain.Model.Aggregates;
using learning_center_webapi.Contexts.Profiles.Interfaces.REST.Resources;

namespace learning_center_webapi.Contexts.Profiles.Interfaces.REST.Transform;

public static class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResource(Profile profile)
    {
        return new ProfileResource
        {
            Id = profile.Id,
            UserId = profile.UserId,
            Nombre = profile.Name.Value,
            Correo = profile.Email.Value,
            Edad = profile.Age.Value,
            Residencia = profile.Residence.Value,
            Telefono = profile.Phone.Value,
            Genero = profile.Gender.Value,
            NivelInstruccion = profile.EducationLevel.Value,
            PhotoDni = profile.PhotoDni,
            PhotoCredential = profile.PhotoCredential,
            Bio = profile.Bio,
            CreatedAt = profile.CreatedDate.ToString("o")
        };
    }
}